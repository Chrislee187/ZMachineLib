using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Or : ZMachineOperation
    {
        public Or(ZMachine2 machine)
            : base((ushort)OpCodes.Or, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var or = (ushort)(args[0] | args[1]);
            var dest = Memory[Stack.Peek().PC++];
            VarHandler.StoreWord(dest, or, true);
        }
    }
}