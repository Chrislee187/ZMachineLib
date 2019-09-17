﻿using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class StoreW : ZMachineOperation
    {
        public StoreW(ZMachine2 machine)
            : base((ushort)OpCodes.StoreW, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + 2 * args[1]);
            ushort value = args[2];
            Machine.Memory.StoreAt(addr, value);
        }
    }
}