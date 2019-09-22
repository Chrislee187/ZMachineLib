using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Managers;

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
        public DecCheck(ZMachine2 machine,
            IZMemory contents,
            IObjectManager objectManager = null,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.DecCheck, null, contents, objectManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)Contents.VariableManager.GetWord((byte)operands[0]);
            val--;
            ushort value = (ushort)val;
            Contents.VariableManager.StoreWord((byte)operands[0], value);
            Jump(val < (short)operands[1]);
        }
    }
}