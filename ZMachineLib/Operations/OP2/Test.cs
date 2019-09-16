using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Test : ZMachineOperation
    {
        public Test(ZMachine2 machine)
            : base((ushort)OpCodes.Test, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump((args[0] & args[1]) == args[1]);
        }
    }
}