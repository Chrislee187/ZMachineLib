using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:135 7 print_addr byte-address-of-string
    /// Print(Z-encoded) string at given byte address, in dynamic or static memory.
    /// </summary>
    public sealed class PrintAddr : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintAddr(IZMemory memory,
            IUserIo io)
            : base((ushort)OpCodes.PrintAddr, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var s = Contents.GetZscii(args[0]);

            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}