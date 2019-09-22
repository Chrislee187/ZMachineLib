using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Jl : ZMachineOperationBase
    {
        public Jl(ZMachine2 machine, IZMemory machineContents)
            : base((ushort)OpCodes.Jl, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Jump((short)operands[0] < (short)operands[1]);
        }
    }
}