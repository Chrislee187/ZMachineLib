using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class ShowStatus : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public ShowStatus(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.ShowStatus, machine, machine.Contents)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands) => _io.ShowStatus();
    }
}