using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RTrue : ZMachineOperationBase
    {
        public RTrue(ZMachine2 machine)
            : base((ushort)OpCodes.RTrue, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            if (Machine.Stack.Pop().StoreResult)
            {
                Contents.VariableManager.StoreWord(GetNextByte(), 1);
            }
        }
    }
}