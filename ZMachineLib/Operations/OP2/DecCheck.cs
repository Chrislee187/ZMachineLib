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
        public DecCheck(ZMachine2 machine)
            : base((ushort)OpCodes.DecCheck, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)VarHandler.GetWord((byte)args[0], true);
            val--;
            ushort value = (ushort)val;
            VarHandler.StoreWord((byte)args[0], value, true);
            Jump(val < (short)args[1]);
        }
    }
}