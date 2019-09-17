using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class PrintPAddr : ZMachineOperation
    {
        private readonly IUserIo _io;

        public PrintPAddr(ZMachine2 machine,
            IUserIo io)
            : base((ushort)OpCodes.PrintObj, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var s = Machine.ZsciiString.GetZsciiString(ObjectManager.GetPackedAddress(args[0]));
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}