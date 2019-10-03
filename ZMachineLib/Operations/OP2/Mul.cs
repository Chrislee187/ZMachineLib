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
        public Mul(IZMemory memory)
            : base((ushort)OpCodes.Mul, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory.GetCurrentByteAndInc();
            var result = args[0] * args[1];

            OpLogging.Op2WithStore(GetType().Name.ToUpper(), args[0], args[1], result, dest);

            Memory.VariableManager.Store(dest, (ushort) result);
        }
    }
}