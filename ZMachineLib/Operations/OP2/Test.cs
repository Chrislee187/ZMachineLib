using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Jump if all of the flags in bitmap are set (i.e. if bitmap & flags == flags).
    /// </summary>
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