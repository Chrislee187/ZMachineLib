using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PrintNum : ZMachineOperation
    {
        private readonly IUserIo _io;

        public PrintNum(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.PrintNum, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var s = args[0].ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}