using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CallVs2 : ZMachineOperation
    {
        public CallVs2(ZMachine2 machine)
            : base((ushort)OpCodes.CallVs2, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Call(operands, true);
        }
    }
}