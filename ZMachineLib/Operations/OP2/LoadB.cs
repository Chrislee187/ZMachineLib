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
            : base((ushort)OpCodes.LoadB, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + operands[1]);
            var dest = GetNextByte();
            Contents.VariableManager.StoreByte(dest, Contents.Manager.Get(addr));
        }
    }
}