using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Inc : ZMachineOperation
    {
        public Inc(ZMachine2 machine)
            : base((ushort)OpCodes.Inc, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(GetVariable((byte)args[0]) + 1);
            StoreWordInVariable((byte)args[0], (ushort)val);
        }
    }
    public sealed class Call1S : ZMachineOperation
    {
        public Call1S(ZMachine2 machine)
            : base((ushort)OpCodes.Call1S, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}