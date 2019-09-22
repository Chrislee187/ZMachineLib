using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class PrintAddr : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintAddr(ZMachine2 machine,
            IUserIo io)
            : base((ushort)OpCodes.PrintAddr, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            var s = ZsciiString.Get(Machine.Memory.AsSpan(operands[0]), Machine.Abbreviations);

            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}