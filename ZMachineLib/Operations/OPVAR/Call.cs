using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Call : ZMachineOperation
    {
        public Call(ZMachine2 machine)
            : base((ushort)OpCodes.Call	, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}