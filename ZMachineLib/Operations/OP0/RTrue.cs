using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RTrue : ZMachineOperationBase
    {
        public RTrue(IZMemory memory)
            : base((ushort)OpCodes.RTrue, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (Memory.Stack.Pop().StoreResult)
            {
                Memory.VariableManager.Store(Memory.GetCurrentByteAndInc(), 1);
            }
        }
    }
}