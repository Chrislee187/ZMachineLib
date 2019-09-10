using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Test : ZMachineOperation
    {
        public Test(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Test, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump((args[0] & args[1]) == args[1]);
        }
    }
}