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

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + 2 * args[1]);
            var dest = Contents.GetCurrentByteAndInc();
            Contents.VariableManager.Store(dest, Contents.Manager.GetUShort(addr));
        }
    }
}