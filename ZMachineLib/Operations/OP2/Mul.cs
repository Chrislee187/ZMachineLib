﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mul : ZMachineOperation
    {
        public Mul(ZMachine2 machine)
            : base((ushort)OpCodes.Mul, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)(operands[0] * operands[1]);
            var dest = GetNextByte();
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}