using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:143 F 5 call_1n routine
    /// Executes routine() and throws away result.
    /// </summary>
    public sealed class Call1N : ZMachineOperationBase
    {
        public Call1N(IZMemory memory)
            : base((ushort)OpCodes.Call1N, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, false);
        }
    }
}