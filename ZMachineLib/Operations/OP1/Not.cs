using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Not : ZMachineOperationBase
    {
        public Not(IZMemory memory)
            : base((ushort)OpCodes.Not, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = Contents.GetCurrentByteAndInc();
            ushort value = (ushort)~operands[0];
            Contents.VariableManager.StoreUShort(dest, value);
        }
    }
}