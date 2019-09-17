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
            var dest = GetNextByte();
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}