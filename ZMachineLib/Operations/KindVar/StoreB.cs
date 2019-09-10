using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class StoreB : ZMachineOperation
    {
        public StoreB(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.StoreB, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + args[1]);
            Memory[addr] = (byte)args[2];
        }
    }
}