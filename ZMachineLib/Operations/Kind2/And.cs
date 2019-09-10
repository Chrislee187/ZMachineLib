using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class And : ZMachineOperation
    {
        public And(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.And, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var and = (ushort)(args[0] & args[1]);
            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, and);
        }
    }
}