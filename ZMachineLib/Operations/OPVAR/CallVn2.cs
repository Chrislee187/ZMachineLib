using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class CallVn2 : ZMachineOperationBase
    {
        public CallVn2(ZMachine2 machine)
            : base((ushort)OpCodes.SoundEffect, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Call(operands, false);
        }
    }
}