using System.Collections.Generic;
using System.Linq;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class Pop : ZMachineOperation
    {
        public Pop(ZMachine2 machine)
            : base((ushort)Kind0OpCodes.Pop, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var routineStack = Stack.Peek().RoutineStack;
            if (routineStack.Any())
            {
                routineStack.Pop();
            }
            else
            {
                Stack.Pop();
            }
        }
    }
}