using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class ReadChar : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public ReadChar(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.ReadChar, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var key = _io.ReadChar();

            var dest = Memory.GetCurrentByteAndInc();
            byte value = (byte)key;
            Memory.VariableManager.Store(dest, value);
        }
    }
}