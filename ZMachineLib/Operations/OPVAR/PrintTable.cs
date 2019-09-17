using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PrintTable : ZMachineOperation
    {
        private readonly IUserIo _io;

        public PrintTable(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.PrintTable, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            // TODO: print properly

            var s = Machine.ZsciiString.GetZsciiString(args[0]);
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}