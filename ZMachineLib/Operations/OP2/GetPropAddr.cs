using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class GetPropAddr : ZMachineOperation
    {
        public GetPropAddr(ZMachine2 machine)
            : base((ushort)OpCodes.GetPropAddr, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            var dest = Memory[Stack.Peek().PC++];
            var addr = GetPropertyAddress(args[0], (byte)args[1]);

            if (addr > 0)
            {
                var propInfo = Memory[addr + 1];

                if (Version > 3 && (propInfo & 0x80) == 0x80)
                    addr += 2;
                else
                    addr += 1;
            }

            VarHandler.StoreWord(dest, addr, true);
        }
    }
}