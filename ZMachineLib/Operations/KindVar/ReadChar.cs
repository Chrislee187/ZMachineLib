﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class ReadChar : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public ReadChar(ZMachine2 machine, IZMachineIo io)
            : base((ushort)KindVarOpCodes.ReadChar, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var key = _io.ReadChar();

            var dest = Memory[Stack.Peek().PC++];
            StoreByteInVariable(dest, (byte)key);
        }
    }
}