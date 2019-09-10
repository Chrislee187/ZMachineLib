using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Add : ZMachineOperation
    {
        public Add(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Add, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(args[0] + args[1]);
            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort)val);
        }
    }
}