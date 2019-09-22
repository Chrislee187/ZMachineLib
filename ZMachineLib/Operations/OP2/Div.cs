using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Div : ZMachineOperationBase
    {
        public Div(ZMachine2 machine,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.Div, machine, variableManager: variableManager)
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