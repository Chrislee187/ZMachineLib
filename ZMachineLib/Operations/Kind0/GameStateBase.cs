﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public abstract class GameStateBase : ZMachineOperation
    {
        protected readonly IZMachineIO Io;
        protected readonly RTrue RTrue;
        protected readonly RFalse RFalse;

        protected GameStateBase(Kind0OpCodes opCode,
            ZMachine2 machine,
            IZMachineIO io,
            RTrue rTrue, RFalse rFalse)
            : base(opCode, machine)
        {
            RFalse = rFalse;
            RTrue = rTrue;

            Io = io;
        }

        public abstract override void Execute(List<ushort> args);

    }
}