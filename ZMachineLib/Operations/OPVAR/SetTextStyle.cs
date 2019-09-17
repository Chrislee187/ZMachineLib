﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class SetTextStyle : ZMachineOperation
    {
        private readonly IUserIo _io;

        public SetTextStyle(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.SetTextStyle, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            _io.SetTextStyle((TextStyle)args[0]);
        }
    }
}