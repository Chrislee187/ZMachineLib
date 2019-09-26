using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class PrintAddr : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintAddr(IZMemory memory,
            IUserIo io)
            : base((ushort)OpCodes.PrintAddr, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            var s = ZsciiString.Get(Contents.Manager.AsSpan(operands[0]), Contents.Abbreviations);

            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}