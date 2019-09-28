using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Call : ZMachineOperationBase
    {
        public Call(IZMemory memory)
            : base((ushort)OpCodes.Call, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}