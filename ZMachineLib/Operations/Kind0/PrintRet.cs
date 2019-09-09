using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class PrintRet : BasePrintingOperations
    {
        private readonly RTrue _rTrue;

        public PrintRet(ZMachine2 machine,
            IZMachineIO io,
            RTrue rTrue)
            : base(Kind0OpCodes.Print, machine, io)
        {
            _rTrue = rTrue;
        }

        public override void Execute(List<ushort> args)
        {
            var s = ZsciiString.GetZsciiString();

            Io.Print(s + Environment.NewLine);
            Log.Write($"[{s}]");
            _rTrue.Execute(null);
        }
    }
}