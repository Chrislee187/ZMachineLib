﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:7 7 test bitmap flags? (label)
    /// Jump if all of the flags in bitmap are set(i.e. if bitmap & flags == flags).
    /// </summary>
    public sealed class Test : ZMachineOperationBase
    {
        public Test(IZMemory memory)
            : base((ushort)OpCodes.Test, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Memory.Jump((args[0] & args[1]) == args[1]);
        }
    }
}