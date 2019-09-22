using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PrintChar : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintChar(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.PrintChar, machine, machine.Contents)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            var s = Convert.ToChar(operands[0]).ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}