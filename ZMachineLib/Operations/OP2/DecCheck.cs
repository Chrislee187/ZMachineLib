using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Decrement variable specified by the first argument,
    /// and branch if it is now less than second argument.
    /// <seealso cref="http://inform-fiction.org/zmachine/standards/z1point1/sect15.html#je"/>
    /// </summary>
    /// 
    public sealed class DecCheck : ZMachineOperationBase
    {
        public DecCheck(IZMemory memory)
            : base((ushort)OpCodes.DecCheck, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)Memory.VariableManager.GetUShort((byte)args[0]);
            val--;
            ushort value = (ushort)val;
            Memory.VariableManager.Store((byte)args[0], value);
            Memory.Jump(val < (short)args[1]);
        }
    }
}