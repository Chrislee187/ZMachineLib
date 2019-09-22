using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mod : ZMachineOperationBase
    {
        public Mod(IZMemory contents)
            : base((ushort)OpCodes.Mod, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)((short)operands[0] % (short)operands[1]);
            var dest = GetNextByte();
            ushort value = (ushort)val;
            Contents.VariableManager.StoreWord(dest, value);
        }
    }
}