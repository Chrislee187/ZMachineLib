using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Load : ZMachineOperationBase
    {
        public Load(ZMachine2 machine)
            : base((ushort)OpCodes.Load, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetNextByte();
            var variableManager = Contents.VariableManager;
            var val = variableManager.GetWord((byte)operands[0], false);
            byte value = (byte)val;
            variableManager.StoreByte(dest, value);
        }
    }
}