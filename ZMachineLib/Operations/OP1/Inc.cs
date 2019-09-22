using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Inc : ZMachineOperationBase
    {
        public Inc(ZMachine2 machine)
            : base((ushort)OpCodes.Inc, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var variableManager = Contents.VariableManager;
            var val = (short)(variableManager.GetWord((byte)operands[0]) + 1);
            ushort value = (ushort)val;
            variableManager.StoreWord((byte)operands[0], value);
        }
    }
}