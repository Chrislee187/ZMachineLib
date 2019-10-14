using System;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public interface IZAttributes
    {
        ulong Value { get; set; }
        void SetAttribute(byte bit);
        void ClearAttribute(byte bit);
        bool TestAttribute(byte bit);
    }

    public class ZAttributes : IZAttributes
    {
        private readonly ushort _attributesLength;
        private readonly ZHeader _header;
        private readonly IMemoryManager _manager;
        private readonly ushort _address;

        public ulong Value
        {
            get
            {
                ulong attr = _manager.GetUInt(_address);

                if (_header.Version > 3)
                {
                    attr = attr << 16 | _manager.GetUShort(_address + sizeof(ushort));
                }

                return attr;
            }
            set
            {
                if (_header.Version <= 3)
                {
                    _manager.SetUInt(_address, (uint) value);
                }
                else
                {
                    _manager.SetUInt(_address, (uint)value >> 16);
                    _manager.SetUShort((uint) (_address+4), (ushort)value);
                }
            }
        }
        public ZAttributes(IMemoryManager manager, ushort address, ZHeader header)
        {
            _address = address;
            _manager = manager;
            _header = header;
            _attributesLength = AttributesLength(header);
        }

        public void SetAttribute(byte bit)
        {
            if (bit > (_attributesLength * 8) - 1) throw new ArgumentOutOfRangeException(nameof(bit), $"Max attr for V{_header.Version} is {AttributesLength(_header) * 8}");
            var bitMask = Mask >> bit;

            Value |= bitMask;
        }
        public void ClearAttribute(byte bit)
        {
            if (bit > (_attributesLength * 8) - 1) throw new ArgumentOutOfRangeException(nameof(bit), $"Max attr for V{_header.Version} is {AttributesLength(_header) * 8}");
            var bitMask = Mask >> bit;

            Value = Value & ~bitMask;
        }
        public bool TestAttribute(byte bit)
        {
            if (bit > (_attributesLength * 8) - 1) throw new ArgumentOutOfRangeException(nameof(bit), $"Max attr for V{_header.Version} is {AttributesLength(_header) * 8}");
            var bitMask = Mask >> bit;

            var test = (Value & bitMask) == bitMask;
            
            return test;
        }
        private ulong Mask => (ulong) (_header.Version <= 3 ? 0x80000000 : 0x800000000000);
        private ushort AttributesLength(ZHeader header) => (ushort) (header.Version <= 3 ? 4 : 6);
    }
}