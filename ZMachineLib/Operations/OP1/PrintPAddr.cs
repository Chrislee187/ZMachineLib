using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class PrintPAddr : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintPAddr(IZMemory memory,
            IUserIo io)
            : base((ushort)OpCodes.PrintObj, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var packedAddress = ZMemory.GetPackedAddress(args[0]);
            var s = ZsciiString.Get(Contents.Manager.AsSpan(packedAddress), 
                Contents.Abbreviations);
            _io.Print(s);
            Log.Write($"[{s}]");


        }
    }
}