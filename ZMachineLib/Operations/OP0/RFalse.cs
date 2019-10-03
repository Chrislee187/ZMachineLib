using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RFalse : ZMachineOperationBase
    {
        public RFalse(IZMemory memory)
            : base((ushort)OpCodes.RFalse, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (Memory.Stack.Pop().StoreResult)
            {
                Memory.VariableManager.Store(Memory.GetCurrentByteAndInc(), 0);
            }
        }
    }
}