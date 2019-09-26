using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RetPopped : ZMachineOperationBase
    {
        public RetPopped(IZMemory memory)
            : base((ushort)OpCodes.RetPopped, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var stackFrame = Contents.Stack.Pop();

            if (stackFrame.StoreResult)
            {
                ushort value = stackFrame.RoutineStack.Pop();
                Contents.VariableManager.StoreWord(GetCurrentByteAndInc(), value);
            }
        }
    }
}