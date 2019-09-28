using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PrintChar : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintChar(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.PrintChar, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var s = Convert.ToChar(args[0]).ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}