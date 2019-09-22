using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class ArtShift : ZMachineOperationBase
    {
        public ArtShift(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.ArtShift, machine)
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

            var dest = PeekNextByte();
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}