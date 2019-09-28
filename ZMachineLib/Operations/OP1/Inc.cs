using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Inc : ZMachineOperationBase
    {
        public Inc(IZMemory memory)
            : base((ushort)OpCodes.Inc, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(Contents.VariableManager.GetUShort((byte)args[0]) + 1);
            ushort value = (ushort)val;
            Contents.VariableManager.Store((byte)args[0], value);
        }
    }
}