using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Mod : ZMachineOperation
    {
        public Mod(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Mod, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)((short)args[0] % (short)args[1]);
            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort)val);
        }
    }
}