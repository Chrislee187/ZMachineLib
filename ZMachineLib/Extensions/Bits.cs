// ReSharper disable ShiftExpressionRealShiftCountIsZero
namespace ZMachineLib.Extensions
{
    public static class Bits
    {
        public const byte Bit0 = 1 << 0;
        public const byte Bit1 = 1 << 1;
        public const byte Bit2 = 1 << 2;
        public const byte Bit3 = 1 << 3;
        public const byte Bit4 = 1 << 4;
        public const byte Bit5 = 1 << 5;
        public const byte Bit6 = 1 << 6;
        public const byte Bit7 = 1 << 7;

        public static bool BitsSet(byte attrs, byte mask)
        {
            return (attrs & mask) == mask;
        }
    }
}