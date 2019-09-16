using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class IncCheck : ZMachineOperation
    {
        public IncCheck(ZMachine2 machine)
            : base((ushort)OpCodes.IncCheck, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)VarHandler.GetWord((byte)args[0], true);
            val++;
            ushort value = (ushort)val;
            VarHandler.StoreWord((byte)args[0], value, true);
            Jump(val > (short)args[1]);
        }
    }
}