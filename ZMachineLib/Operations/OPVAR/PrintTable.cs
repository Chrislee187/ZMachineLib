using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PrintTable : ZMachineOperation
    {
        private readonly IUserIo _io;

        public PrintTable(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.PrintTable, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            // TODO: print properly
            var s = ZsciiString.Get(Machine.Memory.AsSpan(operands[0]), Machine.Abbreviations);
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}