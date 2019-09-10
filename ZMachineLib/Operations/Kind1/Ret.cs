using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class Ret : ZMachineOperation
    {
        public Ret(ZMachine2 machine)
            : base((ushort) Kind1OpCodes.Ret, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var sf = Stack.Pop();
            if (sf.StoreResult)
            {
                var dest = Memory[Stack.Peek().PC++];
                StoreWordInVariable(dest, args[0]);
            }
        }
    }
}