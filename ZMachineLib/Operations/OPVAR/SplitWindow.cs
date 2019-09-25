using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SplitWindow : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public SplitWindow(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.SplitWindow, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.SplitWindow(operands[0]);
        }
    }
}