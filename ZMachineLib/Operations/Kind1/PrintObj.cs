using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class PrintObj : ZMachineOperation
    {
        private readonly IZMachineIO _io;

        public PrintObj(ZMachine2 machine,
            IZMachineIO io)
            : base((ushort)Kind1OpCodes.PrintObj, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            ushort addr = GetPropertyHeaderAddress(args[0]);
            string s = ZsciiString.GetZsciiString((ushort)(addr + 1));
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}