using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Pull : ZMachineOperation
    {
        public Pull(ZMachine2 machine)
            : base((ushort)OpCodes.Pull, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = Machine.Stack.Peek().RoutineStack.Pop();
            VariableManager.StoreWord((byte)operands[0], val, false);
        }
    }
}