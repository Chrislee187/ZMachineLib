﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class ShowStatus : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public ShowStatus(ZMachine2 machine, IZMachineIo io)
            : base((ushort)Kind0OpCodes.ShowStatus, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args) => _io.ShowStatus();
    }
}