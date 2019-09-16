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
            var val = (short)(VarHandler.GetWord((byte)args[0], true) + 1);
            ushort value = (ushort)val;
            VarHandler.StoreWord((byte)args[0], value, true);
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