﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:139 B ret value
    /// Returns from the current routine with the value given.
    /// </summary>
    public sealed class Ret : ZMachineOperationBase
    {
        public Ret(IZMemory memory)
            : base((ushort) OpCodes.Ret, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var sf = Memory.Stack.Pop();
            if (sf.StoreResult)
            {
                var dest = Memory.GetCurrentByteAndInc();
                ushort value = args[0];
                Memory.VariableManager.Store(dest, value);
            }
        }
    }
}