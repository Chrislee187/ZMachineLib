using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Call2N : ZMachineOperation
    {
        public Call2N(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Call2N, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, false);
        }
    }
}