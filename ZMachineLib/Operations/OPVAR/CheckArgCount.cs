using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CheckArgCount : ZMachineOperationBase
    {
        public CheckArgCount(IZMemory memory)
            : base((ushort)OpCodes.CheckArgCount, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Contents.Jump(args[0] <= Contents.Stack.Peek().ArgumentCount);
        }
    }
}