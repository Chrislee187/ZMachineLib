using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Jl : ZMachineOperationBase
    {
        public Jl(ZMachine2 machine)
            : base((ushort)OpCodes.Jl, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Jump((short)operands[0] < (short)operands[1]);
        }
    }
}