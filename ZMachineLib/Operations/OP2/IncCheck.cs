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
            var val = (short)GetVariable((byte)args[0]);
            val++;
            StoreWordInVariable((byte)args[0], (ushort)val);
            Jump(val > (short)args[1]);
        }
    }
}