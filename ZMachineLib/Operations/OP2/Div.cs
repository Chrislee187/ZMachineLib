using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:23 17 div a b -> (result)
    /// Signed 16-bit division.
    /// Division by zero should halt the interpreter with a suitable error message.
    /// </summary>
    public sealed class Div : ZMachineOperationBase
    {
        public Div(IZMemory contents)
            : base((ushort)OpCodes.Div, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetCurrentByteAndInc();

            if (operands[1] == 0)
                return;

            var val = (short)((short)operands[0] / (short)operands[1]);
            ushort value = (ushort)val;
            Contents.VariableManager.StoreUShort(dest, value);
        }
    }
}