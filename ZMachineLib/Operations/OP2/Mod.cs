using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mod : ZMachineOperationBase
    {
        public Mod(IZMemory contents)
            : base((ushort)OpCodes.Mod, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)((short)operands[0] % (short)operands[1]);
            var dest = GetCurrentByteAndInc();
            ushort value = (ushort)val;
            Contents.VariableManager.StoreUShort(dest, value);
        }
    }
}