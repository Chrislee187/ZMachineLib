using System;

namespace ZMachineLib.Extensions
{
    public static class ByteHandlingExtensions
    {
        public static ushort SwapBytes(this ushort value) 
            => (ushort)((value >> 8) | ((value & 0xFF) << 8));

        public static void StoreAt(this byte[] buffer, uint address, uint value)
        {
            buffer[address + 0] = (byte)(value >> 24);
            buffer[address + 1] = (byte)(value >> 16);
            buffer[address + 2] = (byte)(value >> 8);
            buffer[address + 3] = (byte)(value >> 0);
        }

        public static void StoreAt(this byte[] buffer, uint address, ushort value)
        {
            buffer[address + 0] = (byte)(value >> 8);
            buffer[address + 1] = (byte)(value >> 0);
        }

        private static void StoreAt(this byte[] buffer, uint address, byte value)
        {
            buffer[address] = value;
        }

        public static void StoreAt(this byte[] buffer, int address, params byte[] value)
            => StoreAt(buffer, (uint) address, value);

        public static void StoreAt(this byte[] buffer, uint address, params byte[] value)
        {
            uint idx = 0;

            foreach (var b in value)
            {
                buffer.StoreAt(address + idx++, b);
            }
        }
        public static uint GetUInt(this byte[] buffer, uint address)
        {
            var data = buffer.AsSpan((int)address, 4);

            return (uint)(data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3]);
        }
        public static ushort GetUshort(this byte[] buffer, uint address)
        {
            var data = buffer.AsSpan((int)address, 2);

            return (ushort) (data[0] << 8 | data[1]);
        }
    }
}