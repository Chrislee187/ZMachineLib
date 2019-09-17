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
            if (Machine.Stack.Pop().StoreResult)
            {
                VariableManager.StoreWord(Machine.Memory[Machine.Stack.Peek().PC++], 1);
            }
        }
    }
}