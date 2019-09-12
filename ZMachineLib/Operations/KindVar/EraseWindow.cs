﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class EraseWindow : ZMachineOperation
    {
        private readonly IUserIo _io;

        public EraseWindow(ZMachine2 machine, IUserIo io)
            : base((ushort)KindVarOpCodes.EraseWindow, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.EraseWindow(args[0]);
        }
    }
}