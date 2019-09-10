using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Div : ZMachineOperation
    {
        public Div(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Div, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            byte dest = Memory[Stack.Peek().PC++];

            if (args[1] == 0)
                return;

            short val = (short)((short)args[0] / (short)args[1]);
            StoreWordInVariable(dest, (ushort)val);
        }
    }
}