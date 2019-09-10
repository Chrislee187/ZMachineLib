using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class Call : ZMachineOperation
    {
        public Call(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.Call	, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}