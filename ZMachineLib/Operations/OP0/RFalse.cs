using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RFalse : ZMachineOperation
    {
        public RFalse(ZMachine2 machine)
            : base((ushort)OpCodes.RFalse, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (Stack.Pop().StoreResult)
            {
                VariableManager.StoreWord(Memory[Stack.Peek().PC++], 0);
            }
        }
    }
}