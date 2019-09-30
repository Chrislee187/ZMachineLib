using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:21 15 sub a b -> (result)
    /// Signed 16-bit subtraction.
    /// </summary>
    public sealed class Sub : ZMachineOperationBase
    {
        public Sub(IZMemory contents)
            : base((ushort)OpCodes.Sub, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Contents.GetCurrentByteAndInc();
            var result = args[0] - args[1];
           
            OpLogging.Op2WithStore(GetType().Name.ToUpper(), args[0], args[1], result, dest);

            Contents.VariableManager.Store(dest, (ushort) result);
        }
    }
}