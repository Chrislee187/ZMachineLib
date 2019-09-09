using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class Not : ZMachineOperation
    {
        public Not(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.Not, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            byte dest = Machine.Memory[Machine.Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort)~args[0]);
        }
    }
}