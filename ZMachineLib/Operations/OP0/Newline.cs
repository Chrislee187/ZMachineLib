using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Newline : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public Newline(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.NewLine, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands) => _io.Print(Environment.NewLine);
    }
}