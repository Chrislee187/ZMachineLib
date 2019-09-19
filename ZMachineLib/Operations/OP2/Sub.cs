using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:21 15 sub a b -> (result)
    /// Signed 16-bit subtraction.
    /// </summary>
    public sealed class Sub : ZMachineOperation
    {
        public Sub(ZMachine2 machine,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.Sub, machine, variableManager: variableManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)(operands[0] - operands[1]);
            var dest = PeekNextByte();
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}