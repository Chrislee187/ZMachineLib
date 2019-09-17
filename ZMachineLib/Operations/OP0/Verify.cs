using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Verify : ZMachineOperation
    {
        public Verify(ZMachine2 machine)
            : base((ushort)OpCodes.Verify, machine)
        {}

        public override void Execute(List<ushort> operands) => Jump(true);
    }
}