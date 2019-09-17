﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Increment variable, and branch if now greater than value.
    /// </summary>
    public sealed class IncCheck : ZMachineOperation
    {
        public IncCheck(ZMachine2 machine,
            IObjectManager objectManager = null,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.IncCheck, machine, objectManager, variableManager)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)VariableManager.GetWord((byte)args[0], true);
            val++;
            ushort value = (ushort)val;
            VariableManager.StoreWord((byte)args[0], value);
            Jump(val > (short)args[1]);
        }
    }
}