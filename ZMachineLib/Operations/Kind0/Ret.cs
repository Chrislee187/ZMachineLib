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
            ZStackFrame sf = Machine.Stack.Pop();
            if (sf.StoreResult)
            {
                byte dest = Machine.Memory[Machine.Stack.Peek().PC++];
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
            Machine.Stack.Peek().PC = (uint)(Machine.Stack.Peek().PC + (short)(args[0] - 2));
            Log.Write($"-> {Machine.Stack.Peek().PC:X5}");
        }
    }
}