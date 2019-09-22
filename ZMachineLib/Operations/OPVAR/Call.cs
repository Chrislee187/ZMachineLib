using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Call : ZMachineOperationBase
    {
        public Call(ZMachine2 machine)
            : base((ushort)OpCodes.Call	, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Call(operands, true);
        }
    }
}