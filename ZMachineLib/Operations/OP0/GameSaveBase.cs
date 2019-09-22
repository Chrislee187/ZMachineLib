using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public abstract class GameSaveBase : ZMachineOperationBase
    {
        protected readonly IFileIo Io;

        protected GameSaveBase(OpCodes opCode,
            ZMachine2 machine,
            IFileIo io)
            : base((ushort)opCode, machine)
        {
            Io = io;
        }

        public abstract override void Execute(List<ushort> operands);

    }
}