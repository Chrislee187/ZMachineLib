using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mul : ZMachineOperationBase
    {
        public Mul(ZMachine2 machine)
            : base((ushort)OpCodes.Mul, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)(operands[0] * operands[1]);
            var dest = GetNextByte();
            ushort value = (ushort)val;
            Contents.VariableManager.StoreWord(dest, value);
        }
    }
}