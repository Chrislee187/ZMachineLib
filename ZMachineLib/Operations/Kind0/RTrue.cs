﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class RTrue : ZMachineOperation
    {
        public RTrue(ZMachine2 machine)
            : base((ushort)Kind0OpCodes.RTrue, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (Stack.Pop().StoreResult)
            {
                StoreWordInVariable(
                    Memory[Stack.Peek().PC++], 
                    1);
            }
        }
    }
}