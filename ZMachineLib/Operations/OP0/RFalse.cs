using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RFalse : ZMachineOperationBase
    {
        public RFalse(ZMachine2 machine)
            : base((ushort)OpCodes.RFalse, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            if (Machine.Stack.Pop().StoreResult)
            {
                Contents.VariableManager.StoreWord(GetNextByte(), 0);
            }
        }
    }
}