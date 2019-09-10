using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class Print : BasePrintingOperations
    {
        public Print(ZMachine2 machine,
            IZMachineIo io)
            : base((ushort) Kind0OpCodes.Print, machine, io)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var s = ZsciiString.GetZsciiString();
            Io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}