using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Call1S : ZMachineOperationBase
    {
        public Call1S(IZMemory memory)
            : base((ushort)OpCodes.Call1S, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}