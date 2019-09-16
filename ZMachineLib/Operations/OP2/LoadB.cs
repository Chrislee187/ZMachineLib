using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class LoadB : ZMachineOperation
    {
        public LoadB(ZMachine2 machine)
            : base((ushort)OpCodes.LoadB, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + args[1]);
            var b = Memory[addr];
            var dest = Memory[Stack.Peek().PC++];
            VariableManager.StoreByte(dest, b);
        }
    }
}