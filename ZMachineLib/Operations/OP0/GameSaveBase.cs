using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public abstract class GameSaveBase : ZMachineOperationBase
    {
        protected readonly IFileIo Io;

        protected GameSaveBase(OpCodes opCode,
            ZMachine2 machine,
            IZMemory memory,
            IFileIo io)
            : base((ushort)opCode, memory)
        {
            Io = io;
        }

        public abstract override void Execute(List<ushort> operands);

    }
}