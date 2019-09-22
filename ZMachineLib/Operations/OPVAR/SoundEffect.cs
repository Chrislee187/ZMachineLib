using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SoundEffect : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public SoundEffect(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.SoundEffect, machine, machine.Contents)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            // TODO - the rest of the params
            _io.SoundEffect(operands[0]);
        }
    }
}