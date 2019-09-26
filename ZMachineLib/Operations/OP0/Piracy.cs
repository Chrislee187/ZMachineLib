using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Piracy : ZMachineOperationBase
    {
        public Piracy(IZMemory memory)
            : base((ushort)OpCodes.Verify, memory)
        {

        }

        public override void Execute(List<ushort> operands) => Jump(true);
    }
}