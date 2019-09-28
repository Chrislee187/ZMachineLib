using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:16 10 loadb array byte-index -> (result)
    /// Stores array->byte-index(i.e., the byte at address array+byte-index,
    /// which must lie in static or dynamic memory).
    /// </summary>
    public sealed class LoadB : ZMachineOperationBase
    {
        public LoadB(IZMemory contents)
            : base((ushort)OpCodes.LoadB, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + args[1]);
            var dest = Contents.GetCurrentByteAndInc();
            Contents.VariableManager.Store(dest, Contents.Manager.Get(addr));
        }
    }
}