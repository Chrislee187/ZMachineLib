using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SoundEffect : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public SoundEffect(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.SoundEffect, memory)
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