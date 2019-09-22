using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Dec : ZMachineOperationBase
    {
        public Dec(ZMachine2 machine)
            : base((ushort)OpCodes.Dec, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var variableManager = Contents.VariableManager;
            var val = (short)(variableManager.GetWord((byte)operands[0]) - 1);
            ushort value = (ushort)val;
            variableManager.StoreWord((byte)operands[0], value);
        }
    }
}