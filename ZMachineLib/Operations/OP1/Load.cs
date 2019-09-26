using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Load : ZMachineOperationBase
    {
        public Load(IZMemory memory)
            : base((ushort)OpCodes.Load, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = Contents.GetCurrentByteAndInc();
            var val = Contents.VariableManager.GetWord((byte)operands[0], false);
            byte value = (byte)val;
            Contents.VariableManager.StoreByte(dest, value);
        }
    }
}