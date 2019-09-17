using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Print : BasePrintingOperations
    {
        public Print(ZMachine2 machine,
            IUserIo io)
            : base((ushort) OpCodes.Print, machine, io)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var s = Machine.ZsciiString.GetZsciiString();
            Io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}