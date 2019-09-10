using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class Jg : ZMachineOperation
    {
        public Jg(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.Jg, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump((short)args[0] > (short)args[1]);
        }
    }
}