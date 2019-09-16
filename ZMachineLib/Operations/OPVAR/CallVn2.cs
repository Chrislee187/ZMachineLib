using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CallVn2 : ZMachineOperation
    {
        public CallVn2(ZMachine2 machine)
            : base((ushort)OpCodes.SoundEffect, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, false);
        }
    }
}