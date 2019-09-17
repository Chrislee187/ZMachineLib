using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class PrintRet : BasePrintingOperations
    {
        private readonly RTrue _rTrue;

        public PrintRet(ZMachine2 machine,
            IUserIo io,
            RTrue rTrue)
            : base((ushort) OpCodes.PrintRet, machine, io)
        {
            _rTrue = rTrue;
        }

        public override void Execute(List<ushort> args)
        {
            var s = Machine.ZsciiString.GetZsciiString();

            Io.Print(s + Environment.NewLine);
            Log.Write($"[{s}]");
            _rTrue.Execute(null);
        }
    }
}