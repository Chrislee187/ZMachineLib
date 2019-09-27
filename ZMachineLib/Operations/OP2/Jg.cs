using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:3 3 jg a b ?(label)
    /// Jump if a > b(using a signed 16-bit comparison).
    /// </summary>
    public sealed class Jg : ZMachineOperationBase
    {
        public Jg(IZMemory contents)
            : base((ushort)OpCodes.Jg, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Jump((short)operands[0] > (short)operands[1]);
        }
    }
}