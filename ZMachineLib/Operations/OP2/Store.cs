using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Store : ZMachineOperationBase
    {
        public Store(ZMachine2 machine)
            : base((ushort)OpCodes.Store, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            ushort value = operands[1];
            VariableManager.StoreWord((byte)operands[0], value, false);
        }
    }
}