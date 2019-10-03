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

        public override void Execute(List<ushort> args)
        {
            if (args[1] == 0)
            {
                for (var i = 0; i < args[2]; i++)
                    Memory.Manager.Set(args[0] + i, 0);
            }
            else if ((short)args[1] < 0)
            {
                for (var i = 0; i < Math.Abs(args[2]); i++)
                    Memory.Manager.Set(args[1] + i, Memory.Manager.Get(args[0] + i));
            }
            else
            {
                for (var i = Math.Abs(args[2]) - 1; i >= 0; i--)
                    Memory.Manager.Set(args[1] + i, Memory.Manager.Get(args[0] + i));
            }
        }
    }
}