using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Ret : ZMachineOperationBase
    {
        public Ret(IZMemory memory)
            : base((ushort) OpCodes.Ret, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var sf = Contents.Stack.Pop();
            if (sf.StoreResult)
            {
                var dest = Contents.GetCurrentByteAndInc();
                ushort value = operands[0];
                Contents.VariableManager.StoreWord(dest, value);
            }
        }
    }
}