using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Jl : ZMachineOperation
    {
        public Jl(ZMachine2 machine)
            : base((ushort)OpCodes.Jl, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump((short)args[0] < (short)args[1]);
        }
    }
}