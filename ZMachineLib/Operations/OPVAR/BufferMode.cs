using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class BufferMode : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public BufferMode(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.BufferMode, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.BufferMode(operands[0] == 1);
        }
    }
}