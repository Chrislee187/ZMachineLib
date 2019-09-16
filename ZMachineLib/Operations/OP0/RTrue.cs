using System.Collections.Generic;

namespace ZMachineLib.Operations.OP0
{
    public sealed class RTrue : ZMachineOperation
    {
        public RTrue(ZMachine2 machine)
            : base((ushort)OpCodes.RTrue, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (Stack.Pop().StoreResult)
            {
                VariableManager.StoreWord(Memory[Stack.Peek().PC++], 1);
            }
        }
    }
}