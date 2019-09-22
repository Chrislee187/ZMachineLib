using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mul : ZMachineOperationBase
    {
        public Mul(ZMachine2 machine)
            : base((ushort)OpCodes.Mul, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)(operands[0] * operands[1]);
            var dest = PeekNextByte();
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}