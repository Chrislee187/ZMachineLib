﻿using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class ReadChar : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public ReadChar(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.ReadChar, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            var key = _io.ReadChar();

            var dest = Contents.GetCurrentByteAndInc();
            byte value = (byte)key;
            Contents.VariableManager.StoreByte(dest, value);
        }
    }
}