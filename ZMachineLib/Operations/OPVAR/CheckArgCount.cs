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

        public override void Execute(List<ushort> operands)
        {
            Jump(operands[0] <= Contents.Stack.Peek().ArgumentCount);
        }
    }
}