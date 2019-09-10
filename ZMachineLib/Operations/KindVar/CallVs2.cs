using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class CallVs2 : ZMachineOperation
    {
        public CallVs2(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.CallVs2, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Call(args, true);
        }
    }
}