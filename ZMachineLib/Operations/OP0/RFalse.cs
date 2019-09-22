using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RFalse : ZMachineOperationBase
    {
        public RFalse(ZMachine2 machine)
            : base((ushort)OpCodes.RFalse, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            if (Machine.Stack.Pop().StoreResult)
            {
                VariableManager.StoreWord(PeekNextByte(), 0);
            }
        }
    }
}