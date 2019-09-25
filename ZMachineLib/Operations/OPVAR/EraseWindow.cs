using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class EraseWindow : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public EraseWindow(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.EraseWindow, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.EraseWindow(operands[0]);
        }
    }
}