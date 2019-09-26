using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PrintTable : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintTable(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.PrintTable, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            // TODO: print properly
            var s = ZsciiString.Get(Contents.Manager.AsSpan(operands[0]), Contents.Abbreviations);
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}