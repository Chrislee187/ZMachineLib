using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:142 E load(variable) -> (result)
    /// The value of the variable referred to by the operand is stored in the result.
    /// (Inform doesn't use this; see the notes to S 14.)
    /// </summary>
    public sealed class Load : ZMachineOperationBase
    {
        public Load(IZMemory memory)
            : base((ushort)OpCodes.Load, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var variable = (byte)args[0];
            var val = Memory.VariableManager.GetUShort(variable, false);

            var dest = Memory.GetCurrentByteAndInc();
            Memory.VariableManager.Store(dest, val); // NOTE: The original code cast the value to byte, don't know why, seems to work ok
        }
    }
}