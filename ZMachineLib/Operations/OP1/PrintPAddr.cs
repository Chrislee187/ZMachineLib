﻿using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class PrintPAddr : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintPAddr(ZMachine2 machine,
            IUserIo io)
            : base((ushort)OpCodes.PrintObj, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            var data = Machine.Memory.AsSpan((int) ObjectManager.GetPackedAddress(operands[0]));
            var s = ZsciiString.Get(data, Machine.Contents.Abbreviations);
            _io.Print(s);
            Log.Write($"[{s}]");
        }
    }
}