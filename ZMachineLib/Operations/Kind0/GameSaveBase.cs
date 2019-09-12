using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public abstract class GameSaveBase : ZMachineOperation
    {
        protected readonly IFileIo Io;

        protected GameSaveBase(Kind0OpCodes opCode,
            ZMachine2 machine,
            IFileIo io)
            : base((ushort)opCode, machine)
        {
            Io = io;
        }

        public abstract override void Execute(List<ushort> args);

    }
}