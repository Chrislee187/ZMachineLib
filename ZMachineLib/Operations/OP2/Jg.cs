using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Jg : ZMachineOperationBase
    {
        public Jg(IZMemory contents)
            : base((ushort)OpCodes.Jg, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Jump((short)operands[0] > (short)operands[1]);
        }
    }
}