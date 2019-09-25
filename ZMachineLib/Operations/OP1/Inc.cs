using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Inc : ZMachineOperationBase
    {
        public Inc(IZMemory memory)
            : base((ushort)OpCodes.Inc, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)(Contents.VariableManager.GetWord((byte)operands[0]) + 1);
            ushort value = (ushort)val;
            Contents.VariableManager.StoreWord((byte)operands[0], value);
        }
    }
}