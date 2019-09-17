﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Bitwise OR
    /// </summary>
    public sealed class Or : ZMachineOperation
    {
        public Or(ZMachine2 machine,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.Or, machine, variableManager: variableManager)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = GetNextByte();
            VariableManager.StoreWord(dest, (ushort)(args[0] | args[1]));
        }
    }
}