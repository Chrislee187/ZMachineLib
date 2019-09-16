using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Newline : ZMachineOperation
    {
        private readonly IUserIo _io;

        public Newline(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.NewLine, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args) => _io.Print(Environment.NewLine);
    }
}