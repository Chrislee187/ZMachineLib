﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:2 2 jl a b ?(label)
    /// Jump if a < b (using a signed 16-bit comparison).
    /// </summary>
    public sealed class Jl : ZMachineOperationBase
    {
        public Jl(IZMemory contents)
            : base((ushort)OpCodes.Jl, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Jump((short)operands[0] < (short)operands[1]);
        }
    }
}