using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CallVs2 : ZMachineOperationBase
    {
        public CallVs2(IZMemory memory)
            : base((ushort)OpCodes.CallVs2, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}