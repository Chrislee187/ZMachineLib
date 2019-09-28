using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
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