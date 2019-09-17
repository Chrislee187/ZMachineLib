using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Dec : ZMachineOperation
    {
        public Dec(ZMachine2 machine)
            : base((ushort)OpCodes.Dec, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)(VariableManager.GetWord((byte)operands[0]) - 1);
            ushort value = (ushort)val;
            VariableManager.StoreWord((byte)operands[0], value);
        }
    }
}