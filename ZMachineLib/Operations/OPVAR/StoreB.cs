using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class StoreB : ZMachineOperationBase
    {
        public StoreB(IZMemory memory)
            : base((ushort)OpCodes.StoreB, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + operands[1]);
            Contents.Manager.Set(addr, (byte)operands[2]);
        }
    }
}