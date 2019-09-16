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
            ushort value = args[1];
            VariableManager.StoreWord((byte)args[0], value, false);
        }
    }
}