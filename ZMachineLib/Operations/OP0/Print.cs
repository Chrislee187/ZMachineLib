using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Print : BasePrintingOperationsBase
    {
        public Print(IZMemory memory,
            IUserIo io)
            : base((ushort) OpCodes.Print, memory, io)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var array = Contents.Manager.AsSpan((int)Contents.Stack.Peek().PC);

            var zStr = new ZsciiString(array, Contents.Abbreviations);

            Contents.Stack.Peek().PC += zStr.BytesUsed;

            Io.Print(zStr.String);
            Log.Write($"[{zStr.String}]");
        }
    }
}