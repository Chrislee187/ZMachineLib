using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:5 5 inc_chk (variable) value ?(label)
    /// Increment variable, and branch if now greater than value.
    /// </summary>
    public sealed class IncCheck : ZMachineOperationBase
    {
        public IncCheck(IZMemory contents)
            : base((ushort)OpCodes.IncCheck, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var variableManager = Contents.VariableManager;
            var val = (short)variableManager.GetUShort((byte)operands[0]);
            val++;
            ushort value = (ushort)val;
            variableManager.StoreUShort((byte)operands[0], value);
            Jump(val > (short)operands[1]);
        }
    }
}