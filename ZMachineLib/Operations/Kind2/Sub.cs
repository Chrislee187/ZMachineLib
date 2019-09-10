using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Sub : ZMachineOperation
    {
        public Sub(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Sub, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            short val = (short)(args[0] - args[1]);
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort)val);
        }
    }
}