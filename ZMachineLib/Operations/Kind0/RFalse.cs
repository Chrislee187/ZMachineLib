﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class RFalse : ZMachineOperation
    {
        public RFalse(ZMachine2 machine)
            : base((ushort)Kind0OpCodes.RFalse, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (Stack.Pop().StoreResult)
            {
                StoreWordInVariable(
                    Memory[Stack.Peek().PC++], 
                    0);
            }
        }
    }
}