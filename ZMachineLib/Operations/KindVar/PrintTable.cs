using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class PrintTable : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public PrintTable(ZMachine2 machine, IZMachineIo io)
            : base((ushort)KindVarOpCodes.PrintTable, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            // TODO: print properly

            var s = ZsciiString.GetZsciiString(args[0]);
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}