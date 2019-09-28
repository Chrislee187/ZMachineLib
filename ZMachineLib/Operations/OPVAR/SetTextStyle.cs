using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SetTextStyle : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public SetTextStyle(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.SetTextStyle, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.SetTextStyle((TextStyle)args[0]);
        }
    }
}