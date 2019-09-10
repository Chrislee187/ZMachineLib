using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Jl : ZMachineOperation
    {
        public Jl(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Jl, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump((short)args[0] < (short)args[1]);
        }
    }
}