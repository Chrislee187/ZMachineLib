using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class PrintNum : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public PrintNum(ZMachine2 machine, IZMachineIo io)
            : base((ushort)KindVarOpCodes.PrintNum, machine)
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