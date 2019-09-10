using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class LoadB : ZMachineOperation
    {
        public LoadB(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.LoadB, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + args[1]);
            var b = Memory[addr];
            var dest = Memory[Stack.Peek().PC++];
            StoreByteInVariable(dest, b);
        }
    }
}