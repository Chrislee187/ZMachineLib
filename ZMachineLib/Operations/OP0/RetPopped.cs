using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RetPopped : ZMachineOperationBase
    {
        public RetPopped(ZMachine2 machine)
            : base((ushort)OpCodes.RetPopped, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var stackFrame = Machine.Stack.Pop();

            if (stackFrame.StoreResult)
            {
                ushort value = stackFrame.RoutineStack.Pop();
                VariableManager.StoreWord(PeekNextByte(), value);
            }
        }
    }
}