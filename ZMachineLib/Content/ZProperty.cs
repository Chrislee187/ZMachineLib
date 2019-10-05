using System.Diagnostics;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    [DebuggerDisplay("{Number}")]
    public class ZProperty
    {
        private readonly IMemoryManager _manager;



        public byte Number { get; }
        public ushort DataAddress { get; }
        private ushort PropertyAddress { get; }
        private ushort Length { get; }

        private byte[] _data;
        public byte[] Data
        {
            get
            {
                if (DataAddress == 0) return _data; // it's a Default Property so was not retrieved from an objects memory

                return _manager.AsSpan(DataAddress, Length);
            }
            set
            {
                _data = value;
                if (DataAddress == 0) return; // it's a Default Property so no memory address to update
                
                _manager?.Set(DataAddress, value[0]);
                _manager?.Set(DataAddress + 1, value[1]);
            }
        }

        public ushort BytesUsed { get; }
        /// <summary>
        /// Constructor for use when a property is taken from default properties
        /// </summary>
        /// <param name="number"></param>
        /// <param name="data"></param>
        public ZProperty(int number, byte[] data)
        {
            Number = (byte) number;

            _data = data;
            DataAddress = 0;
            
            _manager = null;
        }

        /// <summary>
        /// Constructor that will maintain an objects property in zmemory
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="propAddress"></param>
        public ZProperty(IMemoryManager manager, ushort propAddress)
        {
            _manager = manager;
            PropertyAddress = propAddress;
            var propByte = _manager.Get(PropertyAddress);
            Number = GetPropertyNumber(propByte);
            Length = GetPropertySize(propByte);

            DataAddress = (ushort) (PropertyAddress + 1);
            _data = _manager.AsSpan(DataAddress, 2);
            BytesUsed = (ushort) (Length + 1);
        }
        
        public static ushort GetPropertySize(byte propInfo)
            => (ushort)((propInfo >> (byte)PropertyMasks.PropertySizeShiftV3) + 1);

        public static byte GetPropertyNumber(byte propInfo)
            => (byte)(propInfo & (byte)PropertyMasks.PropertyNumberMaskV3);
    }
}