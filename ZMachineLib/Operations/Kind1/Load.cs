using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class Load : ZMachineOperation
    {
        public Load(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.Load, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory[Stack.Peek().PC++];
            var val = GetVariable((byte)args[0], false);
            StoreByteInVariable(dest, (byte)val);
        }
    }
}