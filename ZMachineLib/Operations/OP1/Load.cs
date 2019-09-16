using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Load : ZMachineOperation
    {
        public Load(ZMachine2 machine)
            : base((ushort)OpCodes.Load, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory[Stack.Peek().PC++];
            var val = VariableManager.GetWord((byte)args[0], false);
            byte value = (byte)val;
            VariableManager.StoreByte(dest, value);
        }
    }
}