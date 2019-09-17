﻿using System.Collections.Generic;

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
            var stackFrame = Machine.Stack.Pop();

            if (stackFrame.StoreResult)
            {
                ushort value = stackFrame.RoutineStack.Pop();
                VariableManager.StoreWord(GetNextByte(), value);
            }
        }
    }
}