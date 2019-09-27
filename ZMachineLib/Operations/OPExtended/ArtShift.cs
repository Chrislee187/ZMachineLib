using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class ArtShift : ZMachineOperationBase
    {
        public ArtShift(IZMemory memory)
            : base((ushort)KindExtOpCodes.ArtShift, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            // keep the sign bit, so make it a short
            var val = (short)operands[0];
            if ((short)operands[1] > 0)
                val <<= operands[1];
            else if ((short)operands[1] < 0)
                val >>= -operands[1];

            var dest = Contents.GetCurrentByteAndInc();
            ushort value = (ushort)val;
            Contents.VariableManager.StoreUShort(dest, value);
        }
    }
}