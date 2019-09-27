using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:21 15 sub a b -> (result)
    /// Signed 16-bit subtraction.
    /// </summary>
    public sealed class Sub : ZMachineOperationBase
    {
        public Sub(IZMemory contents)
            : base((ushort)OpCodes.Sub, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)(operands[0] - operands[1]);
            var dest = GetCurrentByteAndInc();
            ushort value = (ushort)val;
            Contents.VariableManager.StoreUShort(dest, value);
        }
    }
}