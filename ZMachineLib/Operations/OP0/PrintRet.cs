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
            : base((ushort) OpCodes.PrintRet, memory, io)
        {
            _rTrue = rTrue;
        }

        public override void Execute(List<ushort> args)
        {
            var array = Contents.Manager
                .AsSpan((int)Contents.Stack.GetPC());

            var s = Contents.GetZscii(array);

            Io.Print(s + Environment.NewLine);

            _rTrue.Execute(null);
        }
    }
}