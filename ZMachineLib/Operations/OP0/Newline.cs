using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    /// <summary>
    /// 0OP:187 B new_line
    /// Print carriage return.
    /// </summary>
    public sealed class Newline : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public Newline(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.NewLine, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args) => _io.Print(Environment.NewLine);
    }
}