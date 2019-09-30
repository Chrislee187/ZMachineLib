using System;
using ZMachineLib.Extensions;

namespace ZMachineLib.Content
{

    [Flags]
    public enum PropertyMasks : byte
    {
        PropertyNumberMaskV3 = Bits.Bit4 | Bits.Bit3 | Bits.Bit2 | Bits.Bit1 | Bits.Bit0,
        PropertySizeShiftV3 = 5
    }
}