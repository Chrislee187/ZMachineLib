using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Jump if all of the flags in bitmap are set (i.e. if bitmap & flags == flags).
    /// </summary>
    public sealed class Test : ZMachineOperationBase
    {
        public Test(ZMachine2 machine)
            : base((ushort)OpCodes.Test, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Jump((operands[0] & operands[1]) == operands[1]);
        }
    }
}