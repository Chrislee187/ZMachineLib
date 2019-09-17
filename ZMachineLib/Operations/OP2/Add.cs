using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Add : ZMachineOperation
    {
        public Add(ZMachine2 machine)
            : base((ushort)OpCodes.Add, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(args[0] + args[1]);
            var dest = GetNextByte();
            ushort value = (ushort)val;
            VariableManager.StoreWord(dest, value);
        }
    }
}