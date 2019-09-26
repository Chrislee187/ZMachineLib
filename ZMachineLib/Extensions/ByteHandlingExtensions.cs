using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ZMachineLib.Extensions
{
    public static class ByteHandlingExtensions
    {
        public static ushort SwapBytes(this ushort value) 
            => (ushort)((value >> 8) | ((value & 0xFF) << 8));

        public static void SetLong(this byte[] buffer, uint address, uint value)
        {
            buffer[(ushort) address + 0] = (byte)(value >> 24);
            buffer[(ushort) address + 1] = (byte)(value >> 16);
            buffer[(ushort) address + 2] = (byte)(value >> 8);
            buffer[(ushort) address + 3] = (byte)(value >> 0);
        }

        public static void SetWord(this byte[] buffer, uint address, ushort value)
        {
            buffer[address + 0] = (byte)(value >> 8);
            buffer[address + 1] = (byte)(value >> 0);
        }

        private static void StoreAt(this byte[] buffer, uint address, byte value)
        {
            buffer[address] = value;
        }

        public static void StoreAt(this byte[] buffer, int address, params byte[] value)
            => Set(buffer, (uint) address, value);

        public static void Set(this byte[] buffer, uint address, params byte[] value)
        {
            uint idx = 0;

            foreach (var b in value)
            {
                buffer.StoreAt(address + idx++, b);
            }
        }

        public static uint GetUInt(this byte[] buffer, int address)
            => buffer.AsSpan(address, sizeof(uint)).GetUInt();

        public static ushort GetUShort(this Span<byte> data, int address) 
            => data.Slice(address,2).GetUShort();

        public static ushort GetUShort(this byte[] buffer, int address) 
            => GetUShort(buffer.AsSpan(address, 2));
        
        /// <summary>
        /// Gets a short from the next two bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ushort GetUShort(this Span<byte> bytes) 
            => (ushort)(bytes[0] << 8 | bytes[1]);

        /// <summary>
        /// Gets a uint from the next four bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static uint GetUInt(this Span<byte> bytes)
            => (uint)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);

        public static byte FromBitNumber(this byte bitNumber)
            => (byte) (1 << bitNumber);

        public static byte[] ToByteArray(this ushort value)
        {
            var result = new byte[2];

            result[0] = (byte) value;
            result[1] = (byte) (value >> 8);

            return result;
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }
    }

    public class Bits
    {
        public const byte Bit0 = 1 << 0;
        public const byte Bit1 = 1 << 1;
        public const byte Bit2 = 1 << 2;
        public const byte Bit3 = 1 << 3;
        public const byte Bit4 = 1 << 4;
        public const byte Bit5 = 1 << 5;
        public const byte Bit6 = 1 << 6;
        public const byte Bit7 = 1 << 7;
        public static byte ForByte( int bitNumber)
        {
            if (bitNumber < 0 || bitNumber > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(bitNumber), "Byte bit numbers must be between 0 and 7");
            }
            return (byte) (1 << bitNumber);
        }

        public static bool BitsSet(byte attrs, byte mask)
        {
            return (attrs & mask) == mask;
        }
    }
}