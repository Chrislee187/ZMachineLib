using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class LoadW : ZMachineOperation
    {
        public LoadW(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.LoadW, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + 2 * args[1]);
            var word = Machine.Memory.GetUshort(addr);
            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, word);
        }
    }
}