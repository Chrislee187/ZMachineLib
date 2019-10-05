using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ZMachineLib.Extensions;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    [DebuggerDisplay("[{ObjectNumber}] '{Name}'")]
    public class ZMachineObject : IZMachineObject
    {
        public ushort ObjectNumber { get; set; }
        public ushort Address { get; }
        public string Name { get; private set; }

        public uint Attributes 
        {
            get => _manager.AsSpan(Address, 4).GetUInt();
            private set => _manager.SetUInt(Address, value);
        }
        public IDictionary<int, ZProperty> Properties { get; set; }

        public byte BytesRead { get; private set; }

        private readonly IReadOnlyDictionary<int, byte[]> _defaultProps;


        private readonly IMemoryManager _manager;
        private readonly ZAbbreviations _abbreviations;
        private readonly ZHeader _header;
        private VersionedOffsets Offsets => VersionedOffsets.For(_header.Version);


        protected ZMachineObject(in string name, in ushort address, in ushort objectNumber, 
            in ushort parent, in ushort sibling, 
            in ushort child)
        {
            ObjectNumber = objectNumber;
            Address = address;
            // ReSharper disable VirtualMemberCallInConstructor
            Parent = parent;
            Sibling = sibling;
            Child = child;
            // ReSharper restore VirtualMemberCallInConstructor
            Name = name;
        }

        public ZMachineObject(ushort objNumber, ushort address,
            ZHeader header,
            IMemoryManager manager,
            ZAbbreviations abbreviations, 
            IReadOnlyDictionary<int, byte[]> defaultProps)
        {
            _defaultProps = defaultProps;
            if (objNumber == 0) return;

            _header = header;
            _manager = manager;
            _abbreviations = abbreviations;

            ObjectNumber = objNumber;
            Address = address;

            HydrateObject();
        }

        public virtual ZMachineObject RefreshFromMemory()
        {
            HydrateObject();
            return this;
        }

        public ZProperty GetPropertyOrDefault(int i)
        {
            if (!Properties.TryGetValue(i, out var value))
            {
                value = new ZProperty(i, _defaultProps[i]);
            }
            return value;
        }

        private void HydrateObject()
        {
            var ptr = Address;

            ptr += sizeof(uint); // skip Attributes
            ptr += 3; // skip parent/child/sibling

            PropertiesAddress = _manager.GetUShort(ptr);
            ptr += sizeof(ushort);

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

        private Dictionary<int, ZProperty> GetProperties(ushort ptr)
        {
            Debug.Assert(ptr > 0, "GetProperties ptr not > 0");

            var properties = new Dictionary<int, ZProperty>();
            var b = _manager.Get(ptr);
            while (b != 0)
            {
                // Section 12.4.1 - V3 Specific
                var prop = new ZProperty(_manager, ptr);
                properties.Add(prop.Number, prop);
                ptr += prop.BytesUsed;
                b = _manager.Get(ptr);
            }

            return properties;
        }

        public ushort PropertiesAddress { get; set; }

        public virtual ushort Parent
        {
            get => _manager?.Get((ushort) (Address + Offsets.Parent)) ?? 0;
            set => _manager?.Set((ushort) (Address + Offsets.Parent), (byte)value);
        }

        public virtual ushort Sibling
        {
            get => _manager?.Get((ushort) (Address + Offsets.Sibling)) ?? 0;
            set => _manager?.Set((ushort) (Address + Offsets.Sibling), (byte)value);
        }

        public virtual ushort Child
        {
            get => _manager?.Get((ushort) (Address + Offsets.Child)) ?? 0;
            set => _manager?.Set((ushort) (Address + Offsets.Child), (byte)value);
        }

        private readonly Func<ushort, uint> _flagsProviderV3 = attr
            => 0x80000000 >> attr;

        public bool TestAttribute(ushort attr) 
            => (_flagsProviderV3(attr) & Attributes) == _flagsProviderV3(attr);

        public void ClearAttribute(ushort attr)
        {
            var flagMask = _flagsProviderV3(attr);

            if (_header.Version <= 3)
            {
                Attributes = Attributes & ~flagMask;
            }
            else
            {
                // TODO: _manager NOT tested in this section
                Attributes = Attributes & ~flagMask;
                var val = Attributes >> 16;
                _manager.SetUInt(Address, val);
                var value = (ushort)Attributes;
                _manager.Set((ushort)(Address + 4), value);
            }
        }

        public void SetAttribute(ushort attr)
        {
            var flagMask = _flagsProviderV3(attr);

            if (_header.Version <= 3)
            {
                Attributes |= flagMask;
            }
            else
            {
                // TODO: _manager NOT tested in this section
                Attributes |= flagMask;
                _manager.SetUInt(Address, Attributes >> 16);
                _manager.Set((ushort)(Address + 4), (ushort)Attributes);

            }
        }

        #region Equals overrides
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
            return Equals((ZMachineObject)obj);
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

        #endregion

        public static readonly ZMachineObject Object0 = new ZMachineObject(0, 0, default, null, null, null);
        public override string ToString()
            => $"[{ObjectNumber:D3}] ({Address:X4}) '{Name}' ";
    }
}