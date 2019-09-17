using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Decrement variable specified by the first argument,
    /// and branch if it is now less than second argument.
    /// <seealso cref="http://inform-fiction.org/zmachine/standards/z1point1/sect15.html#je"/>
    /// </summary>
    /// 
    public sealed class DecCheck : ZMachineOperation
    {
        public DecCheck(ZMachine2 machine,
            IObjectManager objectManager = null,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.DecCheck, machine, objectManager, variableManager)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)VariableManager.GetWord((byte)args[0]);
            val--;
            ushort value = (ushort)val;
            VariableManager.StoreWord((byte)args[0], value);
            Jump(val < (short)args[1]);
        }
    }
}