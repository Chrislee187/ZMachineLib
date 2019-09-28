using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:1 1 je a b c d ?(label)
    /// Jump if a is equal to any of the subsequent args.
    /// (Thus @je a never jumps and @je a b jumps if a = b.)
    /// je with just 1 operand is not permitted.
    /// </summary>
    public sealed class Je : ZMachineOperationBase
    {
        public Je(IZMemory contents)
            : base((ushort)OpCodes.Je, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var equal = false;
            for (var i = 1; i < args.Count; i++)
            {
                if (args[0] == args[i])
                {
                    equal = true;
                    break;
                }
            }

            Jump(equal);
        }
    }
}