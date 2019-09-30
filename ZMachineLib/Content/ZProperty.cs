using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public class ZProperty
    {
        private readonly IMemoryManager _manager;


        public byte Number { get; }
        public ushort DataAddress { get; }
        public ushort Length => (ushort) Data.Length;
        public byte[] Data { get; set; }

        // NOTE: These int number overload is a little dodgy
        public ZProperty(int number, ushort dataAddress, byte[] data,
            IMemoryManager manager)
        {
            Number = (byte) number;

            Data = data;
            DataAddress = dataAddress;
            
            _manager = manager;
        }
        public ZProperty(byte propByte, ushort dataAddress, byte[] data,
            IMemoryManager manager)
        {
            Number = GetPropertyNumber(propByte);

            Data = data;
            DataAddress = dataAddress;
            
            _manager = manager;
        }

        public static ushort GetPropertySize(byte propInfo)
            => (ushort)((propInfo >> (byte)PropertyMasks.PropertySizeShiftV3) + 1);

        public static byte GetPropertyNumber(byte propInfo)
            => (byte)(propInfo & (byte)PropertyMasks.PropertyNumberMaskV3);
    }
}