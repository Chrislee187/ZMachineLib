using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class And : ZMachineOperation
    {
        public And(ZMachine2 machine)
            : base((ushort)OpCodes.And, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var and = (ushort)(args[0] & args[1]);
            var dest = Memory[Stack.Peek().PC++];
            VarHandler.StoreWord(dest, and, true);
        }
    }
}