using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mul : ZMachineOperation
    {
        public Mul(ZMachine2 machine)
            : base((ushort)OpCodes.Mul, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(args[0] * args[1]);
            var dest = Memory[Stack.Peek().PC++];
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}