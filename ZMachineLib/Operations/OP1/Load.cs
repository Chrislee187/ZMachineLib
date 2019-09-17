using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Load : ZMachineOperation
    {
        public Load(ZMachine2 machine)
            : base((ushort)OpCodes.Load, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = PeekNextByte();
            var val = VariableManager.GetWord((byte)operands[0], false);
            byte value = (byte)val;
            VariableManager.StoreByte(dest, value);
        }
    }
}