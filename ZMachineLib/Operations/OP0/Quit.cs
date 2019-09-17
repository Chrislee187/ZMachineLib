using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Quit : ZMachineOperation
    {
        private readonly IUserIo _io;

        public Quit(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.Quit, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            Machine.Running = false;
            _io.Quit();
        }
    }
}