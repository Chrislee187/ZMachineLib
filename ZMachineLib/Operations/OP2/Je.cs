using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Je : ZMachineOperationBase
    {
        public Je(IZMemory contents)
            : base((ushort)OpCodes.Je, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var equal = false;
            for (var i = 1; i < operands.Count; i++)
            {
                if (operands[0] == operands[i])
                {
                    equal = true;
                    break;
                }
            }

            Jump(equal);
        }
    }
}