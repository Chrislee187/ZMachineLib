using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RetPopped : ZMachineOperation
    {
        public RetPopped(ZMachine2 machine)
            : base((ushort)OpCodes.RetPopped, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var stackFrame = Stack.Pop();

            if (stackFrame.StoreResult)
            {
                ushort value = stackFrame.RoutineStack.Pop();
                VarHandler.StoreWord(Memory[Stack.Peek().PC++], value, true);
            }
        }
    }
}