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

        public override void Execute(List<ushort> operands)
        {
            Contents.Stack.Peek().PC = (uint)(Contents.Stack.Peek().PC + (short)(operands[0] - 2));
            Log.Write($"-> {Contents.Stack.Peek().PC:X5}");
        }
    }
}