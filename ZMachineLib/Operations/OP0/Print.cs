using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Print : BasePrintingOperations
    {
        public Print(ZMachine2 machine,
            IUserIo io)
            : base((ushort) OpCodes.Print, machine, io)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var array = Machine.Memory.AsSpan((int) Machine.Stack.Peek().PC);

            var zStr = new ZsciiString(array, Machine.Abbreviations);

            Machine.Stack.Peek().PC += zStr.BytesUsed;

            Io.Print(zStr.String);
            Log.Write($"[{zStr.String}]");
        }
    }
}