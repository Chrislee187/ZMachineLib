using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CallVn : ZMachineOperation
    {
        public CallVn(ZMachine2 machine)
            : base((ushort)OpCodes.CallVn, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, false);
        }
    }
}