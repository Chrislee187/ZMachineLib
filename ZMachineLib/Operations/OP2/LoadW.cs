using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class LoadW : ZMachineOperationBase
    {
        public LoadW(IZMemory contents)
            : base((ushort)OpCodes.LoadW, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + 2 * operands[1]);
            var dest = GetNextByte();
            Contents.VariableManager.StoreWord(dest, Contents.Manager.GetUShort(addr));
        }
    }
}