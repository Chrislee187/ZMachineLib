using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class Pull : ZMachineOperation
    {
        public Pull(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.Pull, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = Stack.Peek().RoutineStack.Pop();
            StoreWordInVariable((byte)args[0], val, false);
        }
    }
}