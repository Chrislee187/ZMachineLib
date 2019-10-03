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
        public Call2S(IZMemory memory)
            : base((ushort)OpCodes.Call2S, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}