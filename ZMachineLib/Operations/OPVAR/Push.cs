using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Push : ZMachineOperationBase
    {
        public Push(IZMemory memory)
            : base((ushort)OpCodes.Push, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Contents.Stack.Peek().RoutineStack.Push(operands[0]);
        }
    }
}