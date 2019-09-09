using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class Verify : ZMachineOperation
    {
        public Verify(ZMachine2 machine)
            : base((ushort)Kind0OpCodes.Verify, machine)
        {}

        public override void Execute(List<ushort> args) => Jump(true);
    }
}