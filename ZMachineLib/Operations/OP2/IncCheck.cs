using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Increment variable, and branch if now greater than value.
    /// </summary>
    public sealed class IncCheck : ZMachineOperationBase
    {
        public IncCheck(ZMachine2 machine,
            IZMemory contents,
            IObjectManager objectManager = null)
            : base((ushort)OpCodes.IncCheck, machine, contents, objectManager: objectManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var variableManager = Contents.VariableManager;
            var val = (short)variableManager.GetWord((byte)operands[0]);
            val++;
            ushort value = (ushort)val;
            variableManager.StoreWord((byte)operands[0], value);
            Jump(val > (short)operands[1]);
        }
    }
}