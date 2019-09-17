using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class And : ZMachineOperation
    {
        public And(ZMachine2 machine,
            IVariableManager variableManager = null)
            : base((ushort)OpCodes.And, machine, variableManager: variableManager)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = GetNextByte();
            VariableManager.StoreWord(dest, (ushort)(args[0] & args[1]));
        }
    }
}