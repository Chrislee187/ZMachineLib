using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:9 9 and a b -> (result)
    /// Bitwise AND.
    /// </summary>
    public sealed class And : ZMachineOperationBase
    {
        public And(IZMemory contents)
            : base((ushort)OpCodes.And, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetNextByte();
            Contents.VariableManager.StoreWord(dest, (ushort)(operands[0] & operands[1]));
        }
    }
}