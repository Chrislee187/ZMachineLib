using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RFalse : ZMachineOperationBase
    {
        public RFalse(IZMemory memory)
            : base((ushort)OpCodes.RFalse, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            if (Contents.Stack.Pop().StoreResult)
            {
                Contents.VariableManager.StoreWord(Contents.GetCurrentByteAndInc(), 0);
            }
        }
    }
}