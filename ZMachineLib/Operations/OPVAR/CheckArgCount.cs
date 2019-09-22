using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CheckArgCount : ZMachineOperationBase
    {
        public CheckArgCount(ZMachine2 machine)
            : base((ushort)OpCodes.CheckArgCount, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Jump(operands[0] <= Machine.Stack.Peek().ArgumentCount);
        }
    }
}