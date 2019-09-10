using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class Push : ZMachineOperation
    {
        public Push(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.Push, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Stack.Peek().RoutineStack.Push(args[0]);
        }
    }
}