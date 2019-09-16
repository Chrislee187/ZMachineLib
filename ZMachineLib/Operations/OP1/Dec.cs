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
            var val = (short)(GetVariable((byte)args[0]) - 1);
            StoreWordInVariable((byte)args[0], (ushort)val);
        }
    }
}