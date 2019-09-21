using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZMachineLib.Extensions;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public interface IZMachineObject
    {
        bool TestAttribute(ushort attr);
        void ClearAttribute(ushort attr);
        void SetAttribute(ushort attr);
        ulong Attributes { get; }
        string Name { get; }
        ushort Address { get; set; }
        ushort Sibling { get; set; }
        ushort Parent { get; set; }
        ushort Child { get; set; }
        ushort PropertyHeader { get; set; }
        Dictionary<int, bool> AttributeFlags { get; set; }
    }

    [DebuggerDisplay("[{ObjectNumber}] '{Name}'")]
    public class ZMachineObject : IZMachineObject
    {
        public ulong Attributes { get; set; }
        public string Name { get; }

        private readonly Func<ushort, ulong> _flagsProvider;
        private readonly IObjectManager _objectManager;
        public ushort Address { get; set; }

        public ZMachineObject()
        {
            
        }
        public byte BytesRead { get; }
        public ZMachineObject(ushort objNumber, ushort address, Span<byte> dynamicMemory, ZAbbreviations abbreviations)
        {
            ObjectNumber = objNumber;
            Address = address;

            var ptr = address;

            var attrs = dynamicMemory.Slice(ptr, 4).GetUInt();
            SetAttributes(attrs);
            ptr += sizeof(uint);

            Parent = dynamicMemory[ptr++];
            Sibling = dynamicMemory[ptr++];
            Child = dynamicMemory[ptr++];

            PropertiesAddress = dynamicMemory.GetUShort(ptr);
            ptr += 2;

            BytesRead = (byte)(ptr - Address);
            ptr = PropertiesAddress;
            var len = dynamicMemory[ptr++];

            if (len != 0)
            {
                var zStr = new ZsciiString(dynamicMemory.Slice(ptr), abbreviations);
                Name = zStr.String;
            }


            //            var propIndex = 1;
            //            var propSize = (dynamicMemory[ptr++] >> 5) + 1;
            //            while (propSize != 0 && propIndex <= 32 )
            //            {
            //                //                var propValue = 
            //                ptr += (ushort)propSize;
            //                propSize = (dynamicMemory[ptr++] >> 5) + 1;
            //                propIndex++;
            //            }
        }

        private void SetAttributes(uint attrs)
        {
            Attributes = attrs;

            AttributeFlags = new Dictionary<int, bool>();
            for (int i = 1; i <= 32; i++)
            {
                var attrSet = (attrs & (ulong) (1 << i-1)) > 0;
                AttributeFlags.Add(32 - i, attrSet);
            }
        }

        public Dictionary<int, bool> AttributeFlags { get; set; }


        public ushort PropertiesAddress { get; set; }

        public ushort ObjectNumber { get; set; }

        public ZMachineObject(ushort obj, IObjectManager objectManager)
        {
            ObjectNumber = obj;
            // TODO: This needs to be refactored to use a Span<bytes> approach, not the objectManager.GetXXX methods
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
                             | objectManager.Machine.Memory.GetUShort(Address + 4);
                _flagsProvider = attr => (ulong)(0x800000000000 >> attr);
            }

            Child = _objectManager
                .GetObjectNumber((ushort)(
                        Address +
                        _objectManager.Machine.VersionedOffsets.Child)
                );

            if (Child != 0)
            {
                ChildZObject = _objectManager.GetObject(Child);
            }
            Parent =
                _objectManager
                    .GetObjectNumber((ushort)(
                            Address +
                            _objectManager.Machine.VersionedOffsets.Parent)
                    );

            // NOTE: Cannot do ParentZObject, as it blows the stack
//            if (Parent != 0)
//            {
//                ParentZObject = _objectManager.GetObject(Parent);
//            }

            Sibling =
                _objectManager
                    .GetObjectNumber((ushort)(
                            Address +
                            _objectManager.Machine.VersionedOffsets.Sibling)
                    );

            if (Sibling != 0)
            {
                SiblingZObject = _objectManager.GetObject(Sibling);
            }
            //            var versionedOffsetsProperty = Address + _objectManager.Machine.VersionedOffsets.Property;
            //            PropertyHeader = _objectManager.Machine.Memory.GetUShort((ushort) versionedOffsetsProperty);
        }

//        public IZMachineObject ParentZObject { get; set; }

        public IZMachineObject SiblingZObject { get; private set; }

        public ushort Sibling { get; set; }

        public ushort Parent { get; set; }

        public ushort Child { get; set; }
        public IZMachineObject ChildZObject { get; set; }
        public ushort PropertyHeader { get; set; }

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

        public override string ToString() 
            => $"[{ObjectNumber:D3}] ({Address:X4}) '{Name}' ";
    }
}