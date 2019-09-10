using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class PrintAddr : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public PrintAddr(ZMachine2 machine,
            IZMachineIo io)
            : base((ushort)Kind1OpCodes.PrintAddr, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var s = ZsciiString.GetZsciiString(args[0]);
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}