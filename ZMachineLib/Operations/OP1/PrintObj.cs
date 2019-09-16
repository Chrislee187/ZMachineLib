using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class PrintObj : ZMachineOperation
    {
        private readonly IUserIo _io;

        public PrintObj(ZMachine2 machine,
            IUserIo io)
            : base((ushort)OpCodes.PrintObj, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var addr = GetPropertyHeaderAddress(args[0]);
            var s = ZsciiString.GetZsciiString((ushort)(addr + 1));
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}