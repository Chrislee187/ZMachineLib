using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Jump if all of the flags in bitmap are set (i.e. if bitmap & flags == flags).
    /// </summary>
    public sealed class Test : ZMachineOperationBase
    {
        public Test(IZMemory contents)
            : base((ushort)OpCodes.Test, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Jump((operands[0] & operands[1]) == operands[1]);
        }
    }
}