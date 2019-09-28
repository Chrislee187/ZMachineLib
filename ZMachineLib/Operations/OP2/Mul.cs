using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:23 17 div a b -> (result)
    /// Signed 16-bit multiplication.
    /// </summary>

    public sealed class Mul : ZMachineOperationBase
    {
        public Mul(IZMemory contents)
            : base((ushort)OpCodes.Mul, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(args[0] * args[1]);
            var dest = Contents.GetCurrentByteAndInc();
            ushort value = (ushort)val;
            Contents.VariableManager.Store(dest, value);
        }
    }
}