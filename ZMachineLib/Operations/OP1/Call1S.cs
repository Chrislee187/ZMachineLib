using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Call1S : ZMachineOperation
    {
        public Call1S(ZMachine2 machine)
            : base((ushort)OpCodes.Call1S, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}