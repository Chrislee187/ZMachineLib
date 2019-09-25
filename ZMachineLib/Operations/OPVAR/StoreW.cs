using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class StoreW : ZMachineOperationBase
    {
        public StoreW(IZMemory memory)
            : base((ushort)OpCodes.StoreW, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + 2 * operands[1]);
            ushort value = operands[2];
            Contents.Manager.SetWord(addr, value);
        }
    }
}