using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Div : ZMachineOperationBase
    {
        public Div(ZMachine2 machine,
            IZMemory contents)
            : base((ushort)OpCodes.Div, machine, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetNextByte();

            if (operands[1] == 0)
                return;

            var val = (short)((short)operands[0] / (short)operands[1]);
            ushort value = (ushort)val;
            Contents.VariableManager.StoreWord(dest, value);
        }
    }
}