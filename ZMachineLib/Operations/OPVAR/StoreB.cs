﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class StoreB : ZMachineOperationBase
    {
        public StoreB(ZMachine2 machine)
            : base((ushort)OpCodes.StoreB, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + operands[1]);
            MemoryManager.Set(addr, (byte)operands[2]);
        }
    }
}