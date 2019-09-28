using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CallVn : ZMachineOperationBase
    {
        public CallVn(IZMemory memory)
            : base((ushort)OpCodes.CallVn, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, false);
        }
    }
}