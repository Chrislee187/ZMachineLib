using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class CallVn : ZMachineOperation
    {
        public CallVn(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.CallVn, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, false);
        }
    }
}