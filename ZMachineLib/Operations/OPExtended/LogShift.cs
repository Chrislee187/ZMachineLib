﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class LogShift : ZMachineOperation
    {
        public LogShift(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.LogShift, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            // kill the sign bit, so make it a ushort
            var val = args[0];
            if ((short)args[1] > 0)
                val <<= args[1];
            else if ((short)args[1] < 0)
                val >>= -args[1];

            var dest = Machine.Memory[Machine.Stack.Peek().PC++];
            VariableManager.StoreWord(dest, val);
        }
    }
}