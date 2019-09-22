using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RetPopped : ZMachineOperationBase
    {
        public RetPopped(ZMachine2 machine)
            : base((ushort)OpCodes.RetPopped, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var stackFrame = Machine.Stack.Pop();

            if (stackFrame.StoreResult)
            {
                ushort value = stackFrame.RoutineStack.Pop();
                Contents.VariableManager.StoreWord(GetNextByte(), value);
            }
        }
    }
}