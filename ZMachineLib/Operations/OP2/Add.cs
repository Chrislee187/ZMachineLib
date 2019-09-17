using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Add : ZMachineOperation
    {
        public Add(ZMachine2 machine)
            : base((ushort)OpCodes.Add, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)(operands[0] + operands[1]);
            var dest = GetNextByte();
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}