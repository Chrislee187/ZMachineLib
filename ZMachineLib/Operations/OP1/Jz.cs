using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Jz : ZMachineOperation
    {
        public Jz(ZMachine2 machine)
            : base((ushort)OpCodes.Jz, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Jump(operands[0] == 0);
        }
    }
}