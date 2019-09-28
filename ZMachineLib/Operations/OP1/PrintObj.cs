using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class PrintObj : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public PrintObj(IZMemory memory,
            IUserIo io)
            : base((ushort)OpCodes.PrintObj, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var zObj = Contents.ObjectTree.GetOrDefault(obj);
            _io.Print(zObj.Name);
            Log.Write($"[{zObj.Name}]");
        }
    }
}