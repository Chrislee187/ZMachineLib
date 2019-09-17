﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Call1N : ZMachineOperation
    {
        public Call1N(ZMachine2 machine)
            : base((ushort)OpCodes.Call1N, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, false);
        }
    }
}