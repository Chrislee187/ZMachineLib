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

        public override void Execute(List<ushort> operands)
        {
            var addr = ObjectManager.GetPropertyHeaderAddress(operands[0]);
            var s = Machine.ZsciiString.GetZsciiString((ushort)(addr + 1));
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}