using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class LogShift : ZMachineOperation
    {
        public LogShift(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.LogShift, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            // kill the sign bit, so make it a ushort
            var val = operands[0];
            if ((short)operands[1] > 0)
                val <<= operands[1];
            else if ((short)operands[1] < 0)
                val >>= -operands[1];

            var dest = PeekNextByte();
            VariableManager.StoreWord(dest, val);
        }
    }
}