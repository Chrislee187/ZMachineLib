using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:15 F loadw array word-index -> (result)
    /// Stores array-->word-index(i.e., the word at address
    /// array+2*word-index, which must lie in static or dynamic
    /// memory).
    /// </summary>
    public sealed class LoadW : ZMachineOperationBase
    {
        public LoadW(IZMemory contents)
            : base((ushort)OpCodes.LoadW, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + 2 * operands[1]);
            var dest = GetCurrentByteAndInc();
            Contents.VariableManager.StoreUShort(dest, Contents.Manager.GetUShort(addr));
        }
    }
}