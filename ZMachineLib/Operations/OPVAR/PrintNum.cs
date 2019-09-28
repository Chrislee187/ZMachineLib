using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PrintNum : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintNum(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.PrintNum, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var s = args[0].ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}