using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class LogShift : ZMachineOperationBase
    {
        public LogShift(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.LogShift, machine.Memory)
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

            var dest = GetCurrentByteAndInc();
            Contents.VariableManager.StoreWord(dest, val);
        }
    }
}