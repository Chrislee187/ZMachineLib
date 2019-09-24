using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        ushort PropertiesAddress { get; set; }
        IReadOnlyDictionary<int, byte[]> Properties { get;  }
        ZMachineObject RefreshFromMemory();
    }

    [DebuggerDisplay("[{ObjectNumber}] '{Name}'")]
    public class ZMachineObject : IZMachineObject
    {
        public static readonly ZMachineObject Object0 = new ZMachineObject(0, 0, default, null, null);
        public static readonly ZMachineObject Object64 = new ZMachineObject(0, 0, default, null, null);
        public ulong Attributes { get; private set; }
        public IReadOnlyDictionary<int, byte[]> Properties { get; set; }
        public string Name { get; private set; }

        private readonly Func<ushort, ulong> _flagsProviderV3 = attr => 0x80000000 >> attr;

        private readonly IMemoryManager _manager;
        private readonly ZAbbreviations _abbreviations;
        private readonly ZHeader _header;
        private VersionedOffsets Offsets => VersionedOffsets.For(_header.Version);


        public ushort Address { get; set; }

        public ZMachineObject()
        {

        }
        public byte BytesRead { get; private set; }

        public ZMachineObject(ushort objNumber, ushort address,
            ZHeader header,
            IMemoryManager manager,
            ZAbbreviations abbreviations)
        {
            if (objNumber == 0) return;

            _header = header;
            _manager = manager;
            _abbreviations = abbreviations;

            ObjectNumber = objNumber;
            Address = address;

            HydrateObject();

        }

        public ZMachineObject RefreshFromMemory()
        {
            HydrateObject();
            return this;
        }
        private void HydrateObject()
        {
            var ptr = Address;

            var attrs = _manager.AsSpan(ptr, 4).GetUInt();
            SetAttributes(attrs);
            ptr += sizeof(uint);

            Parent = _manager.Get(ptr++);
            Sibling = _manager.Get(ptr++);
            Child = _manager.Get(ptr++);

            PropertiesAddress = _manager.GetUShort(ptr);
            ptr += 2;

            BytesRead = (byte) (ptr - Address);
            ptr = PropertiesAddress;

            var len = _manager.Get(ptr++);

            if (len != 0)
            {
                var zStr = new ZsciiString(_manager.AsSpan(ptr), _abbreviations);
                Name = zStr.String;
                ptr += zStr.BytesUsed;
            }

            Properties = GetProperties(ptr);

        }

        private Dictionary<int, byte[]> GetProperties(ushort ptr)
        {
            // TODO: Need public GetProperty(ushort propNumber)
            //       this will need the default properties table to sorted
            byte GetPropertyNumber(byte sizeByte) 
                => (byte) (sizeByte & (byte) PropertyMasks.PropertyNumberMaskV3 );

            ushort GetActualSize(byte sizeByte) 
                =>(ushort) ((sizeByte >> (byte) PropertyMasks.PropertySizeShiftV3) + 1);

            Debug.Assert(ptr > 0, "GetProperties ptr not > 0");
            var properties = new Dictionary<int, byte[]>();
            while (_manager.Get(ptr) != 0x00)
            {
                var sizeByte = _manager.Get(ptr++);
                var propNum = GetPropertyNumber(sizeByte);
                var propSize = GetActualSize(sizeByte);

                var propData = _manager.AsSpan(ptr, propSize);
                properties.Add(propNum, propData.ToArray());

                ptr += propSize;
            }

            return properties;
        }

        [Flags]
        private enum PropertyMasks : byte
        {
            PropertyNumberMaskV3 = Bits.Bit4 | Bits.Bit3 | Bits.Bit2 | Bits.Bit1 | Bits.Bit0,
            PropertySizeShiftV3 = 5
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

        private ushort _parentObjectNumber;
        public ushort Parent
        {
            get => _parentObjectNumber;
            set
            {
                _parentObjectNumber = value;
                _manager?.Set((ushort)(Address + Offsets.Parent), (byte)value);
            }
        }

        private ushort _childObjectNumber;
        public ushort Child
        {
            get => _childObjectNumber;
            set
            {
                _childObjectNumber = value;
                _manager?.Set((ushort)(Address + Offsets.Child), (byte)value);
            }
        }

        private ushort _siblingObjectNumber;

        public ushort Sibling
        {
            get => _siblingObjectNumber;
            set
            {
                _siblingObjectNumber = value;
                _manager?.Set((ushort)(Address + Offsets.Sibling), (byte)value);
            }
        }

        // TODO: Get object tree to wire these references up
        // ReSharper disable UnassignedGetOnlyAutoProperty
        public IZMachineObject ParentZObject { get; }
        public IZMachineObject SiblingZObject { get; }
        public IZMachineObject ChildZObject { get; }
        // ReSharper restore UnassignedGetOnlyAutoProperty

        public ushort PropertyHeader { get; set; }

        public bool TestAttribute(ushort attr)
        {
            var flags = _flagsProviderV3(attr);
            return (flags & Attributes) == flags;
        }

        public void ClearAttribute(ushort attr)
        {
            var flagMask = _flagsProviderV3(attr);

            if (_header.Version <= 3)
            {
                var attributes = Attributes & ~flagMask;
                _manager.SetLong(Address, (uint)attributes);
            }
            else
            {
                // TODO: _manager NOT tested in this section
                var attributes = Attributes & ~flagMask;
                uint val = (uint)attributes >> 16;
//                _objectManager.Machine.Memory.SetLong(Address, val);
                _manager.SetLong(Address, val);
                ushort value = (ushort)attributes;
//                _objectManager.Machine.Memory.SetWord((ushort)(Address + 4), value);
                _manager.Set((ushort)(Address + 4), value);
            }
        }

        public void SetAttribute(ushort attr)
        {
            var flagMask = _flagsProviderV3(attr);

            if (_header.Version <= 3)
            {
                Attributes |= flagMask;
                _manager.SetLong(Address, (uint)Attributes);

            }
            else
            {
                // TODO: _manager NOT tested in this section
                Attributes |= flagMask;
//                _objectManager.Machine.Memory.SetLong(Address, (uint)(attributes >> 16));
                _manager.SetLong(Address, (uint)(Attributes >> 16));
//                _objectManager.Machine.Memory.SetWord((ushort)(Address + 4), (ushort)attributes);
                _manager.Set((ushort)(Address + 4), (ushort)Attributes);

            }
        }
        public override string ToString() 
            => $"[{ObjectNumber:D3}] ({Address:X4}) '{Name}' ";

        public bool Equals(ZMachineObject other)
        {
            return Attributes == other.Attributes
                   && string.Equals(Name, other.Name)
                   && Address == other.Address 
                   && ObjectNumber == other.ObjectNumber 
                   && Parent == other.Parent 
                   && Sibling == other.Sibling 
                   && Child == other.Child;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            // ReSharper disable once ArrangeThisQualifier
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ZMachineObject) obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Attributes.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Address.GetHashCode();
                hashCode = (hashCode * 397) ^ ObjectNumber.GetHashCode();
                hashCode = (hashCode * 397) ^ Parent.GetHashCode();
                hashCode = (hashCode * 397) ^ Sibling.GetHashCode();
                hashCode = (hashCode * 397) ^ Child.GetHashCode();
                return hashCode;
            }
        }
    }
}