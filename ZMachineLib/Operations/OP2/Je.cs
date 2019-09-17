using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Je : ZMachineOperation
    {
        public Je(ZMachine2 machine)
            : base((ushort)OpCodes.Je, machine)
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