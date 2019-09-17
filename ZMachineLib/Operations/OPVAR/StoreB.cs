using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class StoreB : ZMachineOperation
    {
        public StoreB(ZMachine2 machine)
            : base((ushort)OpCodes.StoreB, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + args[1]);
            Machine.Memory[addr] = (byte)args[2];
        }
    }
}