using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Piracy : ZMachineOperation
    {
        public Piracy(ZMachine2 machine)
            : base((ushort)OpCodes.Verify, machine)
        {

        }

        public override void Execute(List<ushort> operands) => Jump(true);
    }
}