using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class RTrue : ZMachineOperation
    {
        public RTrue(ZMachine2 machine)
            : base(Kind0OpCodes.RTrue, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            if (Machine.Stack.Pop().StoreResult)
            {
                StoreWordInVariable(
                    Machine.Memory[Machine.Stack.Peek().PC++], 
                    1);
            }
        }
    }
}