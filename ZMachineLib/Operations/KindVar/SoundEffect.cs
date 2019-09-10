using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class SoundEffect : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public SoundEffect(ZMachine2 machine, IZMachineIo io)
            : base((ushort)KindVarOpCodes.SoundEffect, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            // TODO - the rest of the params
            _io.SoundEffect(args[0]);
        }
    }
}