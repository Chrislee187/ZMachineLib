using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RTrue : ZMachineOperationBase
    {
        public RTrue(IZMemory memory)
            : base((ushort)OpCodes.RTrue, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            if (Contents.Stack.Pop().StoreResult)
            {
                Contents.VariableManager.StoreWord(Contents.GetCurrentByteAndInc(), 1);
            }
        }
    }
}