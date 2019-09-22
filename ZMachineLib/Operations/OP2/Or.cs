using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Bitwise OR
    /// </summary>
    public sealed class Or : ZMachineOperationBase
    {
        public Or(ZMachine2 machine,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.Or, machine, variableManager: variableManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = PeekNextByte();
            VariableManager.StoreWord(dest, (ushort)(operands[0] | operands[1]));
        }
    }
}