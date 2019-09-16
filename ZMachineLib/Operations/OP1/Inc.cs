using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Inc : ZMachineOperation
    {
        public Inc(ZMachine2 machine)
            : base((ushort)OpCodes.Inc, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)(VariableManager.GetWord((byte)args[0]) + 1);
            ushort value = (ushort)val;
            VariableManager.StoreWord((byte)args[0], value);
        }
    }
}