using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Ret : ZMachineOperation
    {
        public Ret(ZMachine2 machine)
            : base((ushort) OpCodes.Ret, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var sf = Machine.Stack.Pop();
            if (sf.StoreResult)
            {
                var dest = GetNextByte();
                ushort value = operands[0];
                VariableManager.StoreWord(dest, value);
            }
        }
    }
}