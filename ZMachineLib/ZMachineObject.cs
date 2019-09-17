using System;
using ZMachineLib.Extensions;

namespace ZMachineLib
{
    public class ZMachineObject
    {
        public ulong Attributes { get; }
        public string Name { get; }

        private readonly Func<ushort, ulong> _flagsProvider;
        private readonly IObjectManager _objectManager;
        public ushort Address { get; set; }

        public ZMachineObject()
        {
            
        }
        public ZMachineObject(ushort obj, IObjectManager objectManager)
        {
            _objectManager = objectManager;
            // NOTE: Call doesn't seem to do anything and result not used!!
            //            ObjectManager.PrintObjectInfo(obj, false);
            Address = objectManager.GetObjectAddress(obj);
            Name = objectManager.GetObjectName(obj);

            if (objectManager.Machine.Header.Version <= 3)
            {
                Attributes = objectManager.Machine.Memory.GetUInt(Address);
                _flagsProvider = attr => 0x80000000 >> attr;
            }
            else
            {
                Attributes = (ulong)objectManager.Machine.Memory.GetUInt(Address) << 16 
                             | objectManager.Machine.Memory.GetUshort((uint)(Address + 4));
                _flagsProvider = attr => (ulong)(0x800000000000 >> attr);
            }

            Child = _objectManager
                .GetObjectNumber((ushort)(
                        Address +
                        _objectManager.Machine.VersionedOffsets.Child)
                );
            Parent =
                _objectManager
                    .GetObjectNumber((ushort)(
                            Address +
                            _objectManager.Machine.VersionedOffsets.Parent)
                    );
            Sibling =
                _objectManager
                    .GetObjectNumber((ushort)(
                            Address +
                            _objectManager.Machine.VersionedOffsets.Sibling)
                    );
        }

        public ushort Sibling { get; set; }

        public ushort Parent { get; set; }

        public ushort Child { get; set; }

        public bool TestAttribute(ushort attr)
        {
            var flags = _flagsProvider(attr);
            return (flags & Attributes) == flags;
        }

        public void ClearAttribute(ushort attr)
        {
            var flagMask = _flagsProvider(attr);

            if (_objectManager.Machine.Header.Version <= 3)
            {
                var attributes = Attributes & ~flagMask;
                _objectManager.Machine.Memory.StoreAt(Address, (uint)attributes);
            }
            else
            {
                var attributes = Attributes & ~flagMask;
                uint val = (uint)attributes >> 16;
                _objectManager.Machine.Memory.StoreAt(Address, val);
                ushort value = (ushort)attributes;
                _objectManager.Machine.Memory.StoreAt((ushort)(Address + 4), value);
            }
        }

        public void SetAttribute(ushort attr)
        {
            var flagMask = _flagsProvider(attr);

            if (_objectManager.Machine.Header.Version <= 3)
            {
                var attributes = Attributes | flagMask;
                _objectManager.Machine.Memory.StoreAt(Address, (uint)attributes);
            }
            else
            {
                var attributes = Attributes | flagMask;
                _objectManager.Machine.Memory.StoreAt(Address, (uint)(attributes >> 16));
                _objectManager.Machine.Memory.StoreAt((ushort)(Address + 4), (ushort)attributes);
            }
        }


    }
}