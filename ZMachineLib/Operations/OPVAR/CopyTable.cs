using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CopyTable : ZMachineOperationBase
    {
        public CopyTable(ZMachine2 machine)
            : base((ushort)OpCodes.CopyTable, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            if (operands[1] == 0)
            {
                for (var i = 0; i < operands[2]; i++)
                    MemoryManager.Set(operands[0] + i, 0);
            }
            else if ((short)operands[1] < 0)
            {
                for (var i = 0; i < Math.Abs(operands[2]); i++)
                    MemoryManager.Set(operands[1] + i, MemoryManager.Get(operands[0] + i));
            }
            else
            {
                for (var i = Math.Abs(operands[2]) - 1; i >= 0; i--)
                    MemoryManager.Set(operands[1] + i, MemoryManager.Get(operands[0] + i));
            }
        }
    }
}