using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Call2S : ZMachineOperation
    {
        public Call2S(ZMachine2 machine)
            : base((ushort)OpCodes.Call2S, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}