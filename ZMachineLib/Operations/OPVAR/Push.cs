using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Push : ZMachineOperation
    {
        public Push(ZMachine2 machine)
            : base((ushort)OpCodes.Push, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Stack.Peek().RoutineStack.Push(args[0]);
        }
    }
}