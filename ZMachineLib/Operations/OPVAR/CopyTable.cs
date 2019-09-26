using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CopyTable : ZMachineOperationBase
    {
        public CopyTable(IZMemory memory)
            : base((ushort)OpCodes.CopyTable, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            if (operands[1] == 0)
            {
                for (var i = 0; i < operands[2]; i++)
                    Contents.Manager.Set(operands[0] + i, 0);
            }
            else if ((short)operands[1] < 0)
            {
                for (var i = 0; i < Math.Abs(operands[2]); i++)
                    Contents.Manager.Set(operands[1] + i, Contents.Manager.Get(operands[0] + i));
            }
            else
            {
                for (var i = Math.Abs(operands[2]) - 1; i >= 0; i--)
                    Contents.Manager.Set(operands[1] + i, Contents.Manager.Get(operands[0] + i));
            }
        }
    }
}