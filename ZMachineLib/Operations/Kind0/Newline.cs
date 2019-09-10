﻿using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class Newline : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public Newline(ZMachine2 machine, IZMachineIo io)
            : base((ushort)Kind0OpCodes.NewLine, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args) => _io.Print(Environment.NewLine);
    }
}