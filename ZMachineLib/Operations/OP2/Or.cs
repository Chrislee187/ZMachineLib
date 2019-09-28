using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:8 8 or a b -> (result)
    /// Bitwise OR.
    /// </summary>
    public sealed class Or : ZMachineOperationBase
    {
        public Or(IZMemory contents)
            : base((ushort)OpCodes.Or, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Contents.GetCurrentByteAndInc();
            Contents.VariableManager.Store(dest, (ushort)(args[0] | args[1]));
        }
    }
}