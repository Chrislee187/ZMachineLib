using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mod : ZMachineOperation
    {
        public Mod(ZMachine2 machine)
            : base((ushort)OpCodes.Mod, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)((short)args[0] % (short)args[1]);
            var dest = Memory[Stack.Peek().PC++];
            ushort value = (ushort)val;
            VarHandler.StoreWord(dest, value, true);
        }
    }
}