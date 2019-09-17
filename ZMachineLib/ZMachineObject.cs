using System;
using ZMachineLib.Extensions;

namespace ZMachineLib
{
    public class ZMachineObject
    {
        public ulong Attributes { get; }
        public ulong Flags { get; }
        public string Name { get; }
        private readonly Func<ushort, ulong> _flagsProvider;
        private IObjectManager _objectManager;
        public ushort Address { get; }

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
        }

        public bool TestAttribute(ushort attr)
        {
            var flags = _flagsProvider(attr);
            return (flags & Attributes) == flags;
        }

        public ushort Child =>
            _objectManager
                .GetObjectNumber((ushort) (
                    Address + 
                    _objectManager.Machine.VersionedOffsets.Child)
                    );

        public ushort Parent =>
            _objectManager
                .GetObjectNumber((ushort)(
                    Address + 
                    _objectManager.Machine.VersionedOffsets.Parent)
                    );

        public ushort Sibling =>
            _objectManager
                .GetObjectNumber((ushort)(
                        Address +
                        _objectManager.Machine.VersionedOffsets.Sibling)
                );
    }
}