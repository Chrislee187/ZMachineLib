using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Quit : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public Quit(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.Quit, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            Contents.Running = false;
            _io.Quit();
        }
    }
}