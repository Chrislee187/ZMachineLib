using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class SplitWindow : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public SplitWindow(ZMachine2 machine, IZMachineIo io)
            : base((ushort)KindVarOpCodes.SplitWindow, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.SplitWindow(args[0]);
        }
    }
}