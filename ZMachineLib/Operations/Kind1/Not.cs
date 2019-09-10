﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class Not : ZMachineOperation
    {
        public Not(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.Not, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort)~args[0]);
        }
    }
}