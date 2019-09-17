using ZMachineLib.Extensions;

namespace ZMachineLib
{
    public interface IObjectManager
    {
        ZMachine2 Machine { get; }

        void SetObjectNumber(ushort objectAddr, ushort obj);
        ushort GetObjectNumber(ushort objectAddr);
        ushort GetObjectAddress(ushort obj);
        string GetObjectName(ushort obj);
        ushort GetPropertyHeaderAddress(ushort obj);
        ushort GetPropertyAddress(ushort obj, byte prop);
        uint GetPackedAddress(ushort address);
        ushort PrintObjectInfo(ushort obj, bool properties);
        ushort GetObjectParent(ushort objectAddr);
        ZMachineObject GetObject(ushort obj);
    }

    public class ObjectManager : ZMachineHelper, IObjectManager
    {
        public ObjectManager(ZMachine2 machine) : base(machine)
        {
        }
        
        public void SetObjectNumber(ushort objectAddr, ushort obj)
        {
            if (Version <= 3)
                Memory[objectAddr] = (byte)obj;
            else
                Memory.StoreAt(objectAddr, obj);
        }

        public ushort GetObjectNumber(ushort objectAddr)
        {
            if (Version <= 3)
                return Memory[objectAddr];
            return Memory.GetUshort(objectAddr);
        }

        public ushort GetObjectParent(ushort objectAddr) 
            => GetObjectNumber((ushort) (objectAddr + Machine.VersionedOffsets.Parent));

        public ushort GetObjectAddress(ushort obj)
        {
            return (ushort)(ObjectTable 
                            + Offsets.PropertyDefaultTableSize 
                            + (obj - 1) 
                            * Offsets.ObjectSize);
        }

        public string GetObjectName(ushort obj)
        {
            var s = string.Empty;

            if (obj != 0)
            {
                var addr = GetPropertyHeaderAddress(obj);
                if (Memory[addr] != 0)
                {
                    s = ZsciiString.GetZsciiString((uint)(addr + 1));
                }
            }

            return s;
        }

        public ushort GetPropertyHeaderAddress(ushort obj)
        {
            var objectAddr = GetObjectAddress(obj);
            var propAddr = (ushort)(objectAddr + Offsets.Property);
            var prop = Memory.GetUshort(propAddr);
            return prop;
        }

        public ushort GetPropertyAddress(ushort obj, byte prop)
        {
            var propHeaderAddr = GetPropertyHeaderAddress(obj);

            // skip past text
            var size = Memory[propHeaderAddr];
            propHeaderAddr += (ushort)(size * 2 + 1);

            while (Memory[propHeaderAddr] != 0x00)
            {
                var propInfo = Memory[propHeaderAddr];
                var propNum = (byte)(propInfo & (Version <= 3 ? 0x1f : 0x3f));

                if (propNum == prop)
                    return propHeaderAddr;

                byte len;

                if (Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte)(Memory[++propHeaderAddr] & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte)((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

                propHeaderAddr += (ushort)(len + 1);
            }

            return 0;
        }
        
        public uint GetPackedAddress(ushort address)
        {
            if (Version <= 3)
                return (uint)(address * 2);
            if (Version <= 5)
                return (uint)(address * 4);

            return 0;
        }
        
        public ushort PrintObjectInfo(ushort obj, bool properties)
        {
            if (obj == 0)
                return 0;

            var startAddr = GetObjectAddress(obj);

            var attributes = (ulong)Memory.GetUInt(startAddr) << 16 | Memory.GetUshort((uint)(startAddr + 4));
            var parent = GetObjectNumber((ushort)(startAddr + Offsets.Parent));
            var sibling = GetObjectNumber((ushort)(startAddr + Offsets.Sibling));
            var child = GetObjectNumber((ushort)(startAddr + Offsets.Child));
            var propAddr = Memory.GetUshort((uint)(startAddr + Offsets.Property));

            Log.Write($"{obj} ({obj:X2}) at {propAddr:X5}: ");

            var size = Memory[propAddr++];
            var s = string.Empty;
            if (size > 0)
            {
                s = ZsciiString.GetZsciiString(propAddr);
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

                while (Memory[propAddr] != 0x00)
                {
                    var propInfo = Memory[propAddr];
                    byte len;
                    if (Version > 3 && (propInfo & 0x80) == 0x80)
                        len = (byte)(Memory[propAddr + 1] & 0x3f);
                    else
                        len = (byte)((propInfo >> (Version <= 3 ? 5 : 6)) + 1);
                    var propNum = (byte)(propInfo & (Version <= 3 ? 0x1f : 0x3f));

                    Log.Write($"  P:{propNum:X2} at {propAddr:X4}: ");
                    for (var i = 0; i < len; i++)
                        Log.Write($"{Memory[propAddr++]:X2} ");
                    Log.WriteLine("");
                    propAddr++;
                }
            }

            return propAddr;
        }

        public ZMachineObject GetObject(ushort obj)
        {
            return new ZMachineObject(obj, this);
        }
    }
}