using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:141 D print_paddr packed-address-of-string
    /// Print the(Z-encoded) string at the given packed address in high memory.
    /// </summary>
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
            var packedAddress = ZMemory.UnpackedAddress(args[0]);
            var array = Memory.Manager.AsSpan(packedAddress);
            var s = Memory.GetZscii(array);
            _io.Print(s);
            Log.Write($"[{s}]");


        }
    }
}