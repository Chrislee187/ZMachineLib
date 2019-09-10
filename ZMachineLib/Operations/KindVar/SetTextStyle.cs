﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class SetTextStyle : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public SetTextStyle(ZMachine2 machine, IZMachineIo io)
            : base((ushort)KindVarOpCodes.SetTextStyle, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.SetTextStyle((TextStyle)args[0]);
        }
    }
}