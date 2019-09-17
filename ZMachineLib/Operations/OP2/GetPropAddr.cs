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
            Log.Write($"[{ObjectManager.GetObjectName(args[0])}] ");

            var dest = GetNextByte();
            byte prop = (byte)args[1];
            var addr = ObjectManager.GetPropertyAddress(args[0], prop);

            if (addr > 0)
            {
                var propInfo = Machine.Memory[addr + 1];

                if (Machine.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                    addr += 2;
                else
                    addr += 1;
            }

            VariableManager.StoreWord(dest, addr);
        }
    }
}