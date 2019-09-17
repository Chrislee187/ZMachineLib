using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class LoadB : ZMachineOperation
    {
        public LoadB(ZMachine2 machine)
            : base((ushort)OpCodes.LoadB, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + operands[1]);
            var dest = GetNextByte();
            VariableManager.StoreByte(dest, Machine.Memory[addr]);
        }
    }
}