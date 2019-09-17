﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Not : ZMachineOperation
    {
        public Not(ZMachine2 machine)
            : base((ushort)OpCodes.Not, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetNextByte();
            ushort value = (ushort)~operands[0];
            VariableManager.StoreWord(dest, value);
        }
    }
}