using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class Piracy : ZMachineOperation
    {
        public Piracy(ZMachine2 machine)
            : base((ushort)Kind0OpCodes.Verify, machine)
        {

        }

        public override void Execute(List<ushort> args) => Jump(true);
    }
}