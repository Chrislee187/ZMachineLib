using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CallVs2 : ZMachineOperationBase
    {
        public CallVs2(ZMachine2 machine)
            : base((ushort)OpCodes.CallVs2, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Call(operands, true);
        }
    }
}