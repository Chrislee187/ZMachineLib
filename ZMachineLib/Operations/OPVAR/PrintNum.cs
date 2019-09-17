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

        public override void Execute(List<ushort> operands)
        {
            var s = operands[0].ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}