using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public abstract class GameStateBase : ZMachineOperation
    {
        protected readonly IZMachineIo Io;
        protected readonly RTrue RTrue;
        protected readonly RFalse RFalse;

        protected GameStateBase(Kind0OpCodes opCode,
            ZMachine2 machine,
            IZMachineIo io,
            RTrue rTrue, RFalse rFalse)
            : base((ushort)opCode, machine)
        {
            RFalse = rFalse;
            RTrue = rTrue;

            Io = io;
        }

        public abstract override void Execute(List<ushort> args);

    }
}