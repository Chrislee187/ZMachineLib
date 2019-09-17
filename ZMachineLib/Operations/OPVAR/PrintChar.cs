﻿using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PrintChar : ZMachineOperation
    {
        private readonly IUserIo _io;

        public PrintChar(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.PrintChar, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var s = Convert.ToChar(args[0]).ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}