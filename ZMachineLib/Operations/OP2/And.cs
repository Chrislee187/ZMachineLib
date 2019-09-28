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
            : base((ushort)OpCodes.And, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Contents.GetCurrentByteAndInc();
            Contents.VariableManager.Store(dest, (ushort)(args[0] & args[1]));
        }
    }
}