using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Dec : ZMachineOperationBase
    {
        public Dec(IZMemory memory)
            : base((ushort)OpCodes.Dec, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(Contents.VariableManager.GetUShort((byte)args[0]) - 1);
            ushort value = (ushort)val;
            Contents.VariableManager.Store((byte)args[0], value);
        }
    }
}