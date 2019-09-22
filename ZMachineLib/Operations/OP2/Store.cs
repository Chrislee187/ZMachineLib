using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Store : ZMachineOperationBase
    {
        public Store(IZMemory contents)
            : base((ushort)OpCodes.Store, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            ushort value = operands[1];
            Contents.VariableManager.StoreWord((byte)operands[0], value, false);
        }
    }
}