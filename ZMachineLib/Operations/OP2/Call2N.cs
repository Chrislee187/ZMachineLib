using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:26 1A 5 call_2n routine arg1
    /// Executes routine(arg1) and throws away result.
    /// </summary>
    public sealed class Call2N : ZMachineOperationBase
    {
        public Call2N(IZMemory contents)
            : base((ushort)OpCodes.Call2N, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Call(operands, false);
        }
    }
}