using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SetCursor : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public SetCursor(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.SetCursor, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.SetCursor(
                args[0], 
                args[1], 
                (ushort)(args.Count == 3 ? args[2] : 0));

        }
    }
}