using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class PrintPAddr : ZMachineOperation
    {
        private readonly IZMachineIO _io;

        public PrintPAddr(ZMachine2 machine,
            IZMachineIO io)
            : base((ushort)Kind1OpCodes.PrintObj, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            string s = Machine.ZsciiString.GetZsciiString(GetPackedAddress(args[0]));
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}