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
        public DecCheck(IZMemory contents)
            : base((ushort)OpCodes.DecCheck, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)Contents.VariableManager.GetUShort((byte)operands[0]);
            val--;
            ushort value = (ushort)val;
            Contents.VariableManager.StoreUShort((byte)operands[0], value);
            Jump(val < (short)operands[1]);
        }
    }
}