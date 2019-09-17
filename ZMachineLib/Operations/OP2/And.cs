﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:9 9 and a b -> (result)
    /// Bitwise AND.
    /// </summary>
    public sealed class And : ZMachineOperation
    {
        public And(ZMachine2 machine,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.And, machine, variableManager: variableManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = PeekNextByte();
            VariableManager.StoreWord(dest, (ushort)(operands[0] & operands[1]));
        }
    }
}