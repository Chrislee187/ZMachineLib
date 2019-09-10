using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class BufferMode : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public BufferMode(ZMachine2 machine, IZMachineIo io)
            : base((ushort)KindVarOpCodes.BufferMode, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.BufferMode(args[0] == 1);
        }
    }
}