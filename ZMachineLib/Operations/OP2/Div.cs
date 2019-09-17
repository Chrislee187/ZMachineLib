using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Div : ZMachineOperation
    {
        public Div(ZMachine2 machine)
            : base((ushort)OpCodes.Div, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = PeekNextByte();

            if (operands[1] == 0)
                return;

            var val = (short)((short)operands[0] / (short)operands[1]);
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}