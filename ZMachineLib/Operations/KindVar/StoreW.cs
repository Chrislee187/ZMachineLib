using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class StoreW : ZMachineOperation
    {
        public StoreW(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.StoreW, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + 2 * args[1]);
            StoreWord(addr, args[2]);
        }
    }
}