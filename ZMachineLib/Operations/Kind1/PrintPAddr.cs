using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class PrintPAddr : ZMachineOperation
    {
        private readonly IUserIo _io;

        public PrintPAddr(ZMachine2 machine,
            IUserIo io)
            : base((ushort)Kind1OpCodes.PrintObj, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var s = ZsciiString.GetZsciiString(GetPackedAddress(args[0]));
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}