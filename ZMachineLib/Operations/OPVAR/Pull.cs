using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Pull : ZMachineOperationBase
    {
        public Pull(IZMemory memory)
            : base((ushort)OpCodes.Pull, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = Contents.Stack.Peek().RoutineStack.Pop();
            Contents.VariableManager.StoreWord((byte)operands[0], val, false);
        }
    }
}