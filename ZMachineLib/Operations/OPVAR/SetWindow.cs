using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SetWindow : ZMachineOperation
    {
        private readonly IUserIo _io;

        public SetWindow(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.SetWindow, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.SetWindow(args[0]);
        }
    }
}