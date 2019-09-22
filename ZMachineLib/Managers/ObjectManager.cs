using ZMachineLib.Content;

namespace ZMachineLib.Managers
{
    public interface IObjectManager
    {
        void SetObjectNumber(ushort objectAddr, ushort obj);
        ushort GetObjectNumber(ushort objectAddr);
        ushort GetObjectAddress(ushort obj);
        string GetObjectName(ushort obj);
        ushort GetPropertyHeaderAddress(ushort obj);
        ushort GetPropertyAddress(ushort obj, byte prop);
        uint GetPackedAddress(ushort address);
        ushort PrintObjectInfo(ushort obj, bool properties);
        ushort GetObjectParent(ushort objectAddr);
        IZMachineObject GetObject(ushort obj);
    }

    public class ObjectManager : IObjectManager
    {
        // TODO: Refactored in ZMachineObject???
        private readonly IZMemory _memory;

        public ObjectManager(IZMemory memory)
        {
            _memory = memory;
        }

        public IZMachineObject GetObject(ushort obj)
        {
            if (obj == 0)
            {
                return ZMachineObject.Object0;
            }

            var zObj = _memory.ObjectTree[obj].RefreshFromMemory();

            return zObj;
        }

        public void SetObjectNumber(ushort objectAddr, ushort obj)
        {
            if (_memory.Header.Version <= 3)
            {
                _memory.Manager.Set(objectAddr, (byte) obj);
            }
            else
            {
                _memory.Manager.Set(objectAddr, obj);
            }
        
        }

        public ushort GetObjectNumber(ushort objectAddr)
        {
            if (_memory.Header.Version <= 3)
                return _memory.Manager.Get(objectAddr);

            return _memory.Manager.GetUShort(objectAddr);
        }

        public ushort GetObjectParent(ushort objectAddr) 
            => GetObjectNumber((ushort) (objectAddr + _memory.Offsets.Parent));

        public ushort GetObjectAddress(ushort obj)
        {
            return (ushort)(_memory.Header.ObjectTable 
                            + _memory.Offsets.PropertyDefaultTableSize 
                            + (obj - 1) 
                            * _memory.Offsets.ObjectSize);
        }

        public string GetObjectName(ushort obj)
        {
            var s = string.Empty;

            if (obj != 0)
            {
                var addr = GetPropertyHeaderAddress(obj);
                if (_memory.Manager.Get(addr) != 0)
                {
                    s = ZsciiString.Get(_memory.Manager.AsSpan((ushort) (addr + 0x01)), _memory.Abbreviations); 
                }
            }

            return s;
        }

        public ushort GetPropertyHeaderAddress(ushort obj)
        {
            var objectAddr = GetObjectAddress(obj);
            var propAddr = (ushort)(objectAddr + _memory.Offsets.Property);
            var prop = _memory.Manager.GetUShort(propAddr);
            return prop;
        }

        public ushort GetPropertyAddress(ushort obj, byte prop)
        {
            var propHeaderAddr = GetPropertyHeaderAddress(obj);

            // skip past text
            var size = _memory.Manager.Get(propHeaderAddr);
            propHeaderAddr += (ushort)(size * 2 + 1);

            while (_memory.Manager.Get(propHeaderAddr) != 0x00)
            {
                var propInfo = _memory.Manager.Get(propHeaderAddr);
                var propNum = (byte)(propInfo & (_memory.Header.Version <= 3 ? 0x1f : 0x3f));

                if (propNum == prop)
                    return propHeaderAddr;

                byte len;

                if (_memory.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte)(_memory.Manager.Get(++propHeaderAddr) & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte)((propInfo >> (_memory.Header.Version <= 3 ? 5 : 6)) + 1);

                propHeaderAddr += (ushort)(len + 1);
            }

            return 0;
        }

        public uint GetPackedAddress(ushort address)
        {
            if (_memory.Header.Version <= 3)
                return (uint)(address * 2);
            if (_memory.Header.Version <= 5)
                return (uint)(address * 4);

            return 0;
        }

        public ushort PrintObjectInfo(ushort obj, bool properties)
        {
            if (obj == 0)
                return 0;

            var startAddr = GetObjectAddress(obj);

            var attributes = (ulong)_memory.Manager.GetUInt(startAddr) << 16 
                             | _memory.Manager.GetUShort(startAddr + 4);
            var parent = GetObjectNumber((ushort)(startAddr + _memory.Offsets.Parent));
            var sibling = GetObjectNumber((ushort)(startAddr + _memory.Offsets.Sibling));
            var child = GetObjectNumber((ushort)(startAddr + _memory.Offsets.Child));
            var propAddr = _memory.Manager.GetUShort(startAddr + _memory.Offsets.Property);

            Log.Write($"{obj} ({obj:X2}) at {propAddr:X5}: ");

            var size = _memory.Manager.Get(propAddr++);
            var s = string.Empty;
            if (size > 0)
            {
                s = ZsciiString.Get(_memory.Manager.AsSpan(propAddr), _memory.Abbreviations); // s = ZsciiString.GetZsciiString(propAddr);
            }

            propAddr += (ushort)(size * 2);

            Log.WriteLine(
                $"[{s}] A:{attributes:X12} P:{parent}({parent:X2}) ZsciiString:{sibling}({sibling:X2}) C:{child}({child:X2})");

            if (properties)
            {
                var ss = string.Empty;
                for (var i = 47; i >= 0; i--)
                {
                    if (((attributes >> i) & 0x01) == 0x01)
                    {
                        ss += 47 - i + ", ";
                    }
                }

                Log.WriteLine("Attributes: " + ss);

                while (_memory.Manager.Get(propAddr) != 0x00)
                {
                    var propInfo = _memory.Manager.Get(propAddr);
                    byte len;
                    if (_memory.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                        len = (byte)(_memory.Manager.Get(propAddr + 1) & 0x3f);
                    else
                        len = (byte)((propInfo >> (_memory.Header.Version <= 3 ? 5 : 6)) + 1);
                    var propNum = (byte)(propInfo & (_memory.Header.Version <= 3 ? 0x1f : 0x3f));

                    Log.Write($"  P:{propNum:X2} at {propAddr:X4}: ");
                    for (var i = 0; i < len; i++)
                        Log.Write($"{_memory.Manager.Get(propAddr++):X2} ");
                    Log.WriteLine("");
                    propAddr++;
                }
            }

            return propAddr;
        }


    }
}