using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Call2N : ZMachineOperation
    {
        public Call2N(ZMachine2 machine)
            : base((ushort)OpCodes.Call2N, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Call(operands, false);
        }
    }
}