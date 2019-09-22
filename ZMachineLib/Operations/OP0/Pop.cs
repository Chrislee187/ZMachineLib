using System.Collections.Generic;
using System.Linq;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Pop : ZMachineOperationBase
    {
        public Pop(ZMachine2 machine)
            : base((ushort)OpCodes.Pop, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var routineStack = Machine.Stack.Peek().RoutineStack;
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