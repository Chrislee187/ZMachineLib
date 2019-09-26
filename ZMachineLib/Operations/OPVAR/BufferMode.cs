﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class BufferMode : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public BufferMode(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.BufferMode, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.BufferMode(operands[0] == 1);
        }
    }
}