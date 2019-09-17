﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Jump if object a is a direct child of b, i.e., if parent of a is b.
    /// </summary>
    public sealed class Jin : ZMachineOperation
    {
        public Jin(ZMachine2 machine,
            IObjectManager objectManager = null,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.Jin, machine, objectManager, variableManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = ObjectManager.GetObject(operands[0]);

            Jump(zObj.Parent == operands[1]);
        }
    }
}