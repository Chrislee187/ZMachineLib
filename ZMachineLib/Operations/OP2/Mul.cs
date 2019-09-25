using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mul : ZMachineOperationBase
    {
        public Mul(IZMemory contents)
            : base((ushort)OpCodes.Mul, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)(operands[0] * operands[1]);
            var dest = GetCurrentByteAndInc();
            ushort value = (ushort)val;
            Contents.VariableManager.StoreWord(dest, value);
        }
    }
}