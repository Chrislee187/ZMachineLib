using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Div : ZMachineOperation
    {
        public Div(ZMachine2 machine)
            : base((ushort)OpCodes.Div, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = GetNextByte();

            if (args[1] == 0)
                return;

            var val = (short)((short)args[0] / (short)args[1]);
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}