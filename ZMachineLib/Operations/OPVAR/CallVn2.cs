using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CallVn2 : ZMachineOperationBase
    {
        public CallVn2(IZMemory memory)
            : base((ushort)OpCodes.SoundEffect, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, false);
        }
    }
}