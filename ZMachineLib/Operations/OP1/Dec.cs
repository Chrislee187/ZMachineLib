using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Dec : ZMachineOperation
    {
        public Dec(ZMachine2 machine)
            : base((ushort)OpCodes.Dec, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(VarHandler.GetWord((byte)args[0], true) - 1);
            ushort value = (ushort)val;
            VarHandler.StoreWord((byte)args[0], value, true);
        }
    }
}