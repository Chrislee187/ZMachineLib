﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class IncCheck : ZMachineOperation
    {
        public IncCheck(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.IncCheck, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)GetVariable((byte)args[0]);
            val++;
            StoreWordInVariable((byte)args[0], (ushort)val);
            Jump(val > (short)args[1]);
        }
    }
}