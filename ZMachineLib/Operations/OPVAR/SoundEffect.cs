using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SoundEffect : ZMachineOperation
    {
        private readonly IUserIo _io;

        public SoundEffect(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.SoundEffect, machine)
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