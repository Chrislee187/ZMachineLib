using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:138 A print_obj object
    /// Print short name of object (the Z-encoded string in the object header,
    /// not a property). If the object number is invalid, the interpreter should
    /// halt with a suitable error message.
    /// </summary>
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