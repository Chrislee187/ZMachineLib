using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:20 14 add a b -> (result)
    /// Signed 16-bit addition.
    /// </summary>
    public sealed class Add : ZMachineOperationBase
    {
        public Add(ZMachine2 machine, IVariableManager variableManager = null)
            : base((ushort)OpCodes.Add, machine, variableManager: variableManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = PeekNextByte();
            var val = (short)(operands[0] + operands[1]);
            VariableManager.StoreWord(
                dest, 
                (ushort) val
                );
        }
    }
}