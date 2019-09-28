using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Store : ZMachineOperationBase
    {
        public Store(IZMemory contents)
            : base((ushort)OpCodes.Store, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            ushort value = args[1];
            Contents.VariableManager.Store((byte)args[0], value, false);
        }
    }
}