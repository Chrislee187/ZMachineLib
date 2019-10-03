using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:134 6 dec (variable)
    /// Decrement variable by 1. This is signed, so 0 decrements to -1.
    /// </summary>
    public sealed class Dec : ZMachineOperationBase
    {
        public Dec(IZMemory memory)
            : base((ushort)OpCodes.Dec, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(Memory.VariableManager.GetUShort((byte)args[0]) - 1);
            ushort value = (ushort)val;
            Memory.VariableManager.Store((byte)args[0], value);
        }
    }
}