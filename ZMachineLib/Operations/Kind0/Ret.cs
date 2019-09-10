using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind0
{
    public sealed class Ret : ZMachineOperation
    {
        private readonly RTrue _rTrue;

        public Ret(ZMachine2 machine)
            : base((ushort) Kind1OpCodes.Ret, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var sf = Stack.Pop();
            if (sf.StoreResult)
            {
                var dest = Memory[Stack.Peek().PC++];
                StoreWordInVariable(dest, args[0]);
            }
        }
    }
    public sealed class Jump : ZMachineOperation
    {
        public Jump(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.Jump, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Stack.Peek().PC = (uint)(Stack.Peek().PC + (short)(args[0] - 2));
            Log.Write($"-> {Stack.Peek().PC:X5}");
        }
    }
}