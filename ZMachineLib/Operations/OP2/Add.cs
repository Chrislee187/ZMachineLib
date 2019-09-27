using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:20 14 add a b -> (result)
    /// Signed 16-bit addition.
    /// </summary>
    public sealed class Add : ZMachineOperationBase
    {
        public Add(IZMemory contents)
            : base((ushort)OpCodes.Add, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetCurrentByteAndInc();
            var val = (short)(operands[0] + operands[1]);
            Contents.VariableManager.StoreUShort(
                dest, 
                (ushort) val
                );
        }
    }
}