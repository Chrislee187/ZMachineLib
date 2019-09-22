using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Call1N : ZMachineOperationBase
    {
        public Call1N(ZMachine2 machine)
            : base((ushort)OpCodes.Call1N, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Call(operands, false);
        }
    }
}