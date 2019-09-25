using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SetWindow : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public SetWindow(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.SetWindow, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            _io.SetWindow(operands[0]);
        }
    }
}