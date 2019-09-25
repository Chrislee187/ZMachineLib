using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class PrintRet : BasePrintingOperationsBase
    {
        private readonly RTrue _rTrue;

        public PrintRet(IZMemory memory,
            IUserIo io,
            RTrue rTrue)
            : base((ushort) OpCodes.PrintRet, null, memory, io)
        {
            _rTrue = rTrue;
        }

        public override void Execute(List<ushort> operands)
        {
            var array = Contents.Manager.AsSpan((int)Contents.Stack.Peek().PC);

            var s = ZsciiString.Get(array, Contents.Abbreviations);

            Io.Print(s + Environment.NewLine);

            _rTrue.Execute(null);
        }
    }
}