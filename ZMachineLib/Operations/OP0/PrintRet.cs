using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class PrintRet : BasePrintingOperationsBase
    {
        private readonly RTrue _rTrue;

        public PrintRet(ZMachine2 machine,
            IUserIo io,
            RTrue rTrue)
            : base((ushort) OpCodes.PrintRet, machine, io)
        {
            _rTrue = rTrue;
        }

        public override void Execute(List<ushort> operands)
        {
            var array = Machine.Memory.AsSpan((int)Machine.Stack.Peek().PC);

            var s = ZsciiString.Get(array, Machine.Contents.Abbreviations);

            Io.Print(s + Environment.NewLine);

            _rTrue.Execute(null);
        }
    }
}