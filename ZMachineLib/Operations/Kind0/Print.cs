using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class Print : BasePrintingOperations
    {
        public Print(ZMachine2 machine,
            IZMachineIO io)
            : base(Kind0OpCodes.Print, machine, io)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var s = GetZsciiString();
            Io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}