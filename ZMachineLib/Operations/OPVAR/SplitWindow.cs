using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SplitWindow : ZMachineOperation
    {
        private readonly IUserIo _io;

        public SplitWindow(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.SplitWindow, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.SplitWindow(args[0]);
        }
    }
}