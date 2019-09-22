using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Increment variable, and branch if now greater than value.
    /// </summary>
    public sealed class IncCheck : ZMachineOperationBase
    {
        public IncCheck(ZMachine2 machine,
            IObjectManager objectManager = null,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.IncCheck, machine, objectManager, variableManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)VariableManager.GetWord((byte)operands[0]);
            val++;
            ushort value = (ushort)val;
            VariableManager.StoreWord((byte)operands[0], value);
            Jump(val > (short)operands[1]);
        }
    }
}