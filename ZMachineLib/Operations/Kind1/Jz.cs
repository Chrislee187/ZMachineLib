using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class Jz : ZMachineOperation
    {
        public Jz(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.Jz, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump(args[0] == 0);
        }
    }
}