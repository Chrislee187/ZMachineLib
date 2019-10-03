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
        public Or(IZMemory memory)
            : base((ushort)OpCodes.Or, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory.GetCurrentByteAndInc();

            var result = args[0] | args[1];
            OpLogging.Op2WithStore(GetType().Name.ToUpper(), args[0], args[1], result, dest);

            Memory.VariableManager.Store(dest, (ushort)result);
        }
    }
}