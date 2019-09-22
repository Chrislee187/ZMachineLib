using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Newline : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public Newline(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.NewLine, machine, machine.Contents)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands) => _io.Print(Environment.NewLine);
    }
}