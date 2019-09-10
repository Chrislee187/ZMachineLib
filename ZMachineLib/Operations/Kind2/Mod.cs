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
            short val = (short)((short)args[0] % (short)args[1]);
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort)val);
        }
    }
}