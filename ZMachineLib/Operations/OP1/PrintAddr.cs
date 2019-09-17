using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class PrintAddr : ZMachineOperation
    {
        private readonly IUserIo _io;

        public PrintAddr(ZMachine2 machine,
            IUserIo io)
            : base((ushort)OpCodes.PrintAddr, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            var s = Machine.ZsciiString.GetZsciiString(operands[0]);
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}