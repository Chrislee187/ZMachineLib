using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Or : ZMachineOperation
    {
        public Or(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Or, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var or = (ushort)(args[0] | args[1]);
            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, or);
        }
    }
}