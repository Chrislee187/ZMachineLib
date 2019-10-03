using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Store : ZMachineOperationBase
    {
        public Store(IZMemory memory)
            : base((ushort)OpCodes.Store, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            ushort value = args[1];
            Memory.VariableManager.Store((byte)args[0], value, false);
        }
    }
}