﻿using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CopyTable : ZMachineOperation
    {
        public CopyTable(ZMachine2 machine)
            : base((ushort)OpCodes.CopyTable, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (args[1] == 0)
            {
                for (var i = 0; i < args[2]; i++)
                    Machine.Memory[args[0] + i] = 0;
            }
            else if ((short)args[1] < 0)
            {
                for (var i = 0; i < Math.Abs(args[2]); i++)
                    Machine.Memory[args[1] + i] = Machine.Memory[args[0] + i];
            }
            else
            {
                for (var i = Math.Abs(args[2]) - 1; i >= 0; i--)
                    Machine.Memory[args[1] + i] = Machine.Memory[args[0] + i];
            }
        }
    }
}