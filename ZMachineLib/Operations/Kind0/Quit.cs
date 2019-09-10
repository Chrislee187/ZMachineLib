using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class Quit : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public Quit(ZMachine2 machine, IZMachineIo io)
            : base((ushort)Kind0OpCodes.Quit, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            Machine.Running = false;
            _io.Quit();
        }
    }
}