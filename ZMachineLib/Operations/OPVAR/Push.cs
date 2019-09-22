using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Push : ZMachineOperationBase
    {
        public Push(ZMachine2 machine)
            : base((ushort)OpCodes.Push, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Machine.Stack.Peek().RoutineStack.Push(operands[0]);
        }
    }
}