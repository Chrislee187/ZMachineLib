using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
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