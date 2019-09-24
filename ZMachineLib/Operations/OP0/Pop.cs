using System.Collections.Generic;
using System.Linq;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Pop : ZMachineOperationBase
    {
        public Pop(IZMemory contents)
            : base((ushort)OpCodes.Pop, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var routineStack = Contents.Stack.Peek().RoutineStack;
            if (routineStack.Any())
            {
                routineStack.Pop();
            }
            else
            {
                Machine.Stack.Pop();
            }
        }
    }
}