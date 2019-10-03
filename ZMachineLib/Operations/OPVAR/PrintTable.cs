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

        public override void Execute(List<ushort> args)
        {
            // TODO: print properly
            var s = Memory.GetZscii(Memory.Manager.AsSpan(args[0]));
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}