using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Store : ZMachineOperation
    {
        public Store(ZMachine2 machine)
            : base((ushort)OpCodes.Store, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            StoreWordInVariable((byte)args[0], args[1], false);
        }
    }
}