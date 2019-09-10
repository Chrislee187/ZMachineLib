using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Mul : ZMachineOperation
    {
        public Mul(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Mul, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(args[0] * args[1]);
            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort)val);
        }
    }
}