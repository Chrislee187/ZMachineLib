using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Jump : ZMachineOperationBase
    {
        public Jump(IZMemory memory)
            : base((ushort)OpCodes.Jump, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var offset = (short)(args[0] - 2);
            Contents.Stack.IncrementPC(offset);
            Log.Write($"-> {Contents.Stack.GetPC():X5}");
        }
    }
}