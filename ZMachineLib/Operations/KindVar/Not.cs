using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class Not : ZMachineOperation
    {
        public Not(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.Not, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort)~args[0]);
        }
    }
}