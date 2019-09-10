using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class CallVn2 : ZMachineOperation
    {
        public CallVn2(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.SoundEffect, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, false);
        }
    }
}