using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:25 19 4 call_2s routine arg1 -> (result)
    /// Stores routine(arg1).
    /// </summary>
    public sealed class Call2S : ZMachineOperationBase
    {
        public Call2S(IZMemory contents)
            : base((ushort)OpCodes.Call2S, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Call(operands, true);
        }
    }
}