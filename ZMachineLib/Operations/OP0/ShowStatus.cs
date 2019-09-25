using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class ShowStatus : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public ShowStatus(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.ShowStatus, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands) => _io.ShowStatus();
    }
}