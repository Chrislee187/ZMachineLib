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

        public override void Execute(List<ushort> args)
        {
            var val = Memory.Stack.PopCurrentRoutine();
            Memory.VariableManager.Store((byte)args[0], val, false);
        }
    }
}