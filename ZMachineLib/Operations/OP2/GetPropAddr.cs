using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class GetPropAddr : ZMachineOperation
    {
        public GetPropAddr(ZMachine2 machine)
            : base((ushort)OpCodes.GetPropAddr, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Log.Write($"[{ObjectManager.GetObjectName(operands[0])}] ");

            var dest = PeekNextByte();
            byte prop = (byte)operands[1];
            var addr = ObjectManager.GetPropertyAddress(operands[0], prop);

            if (addr > 0)
            {
                var propInfo = MemoryManager.Get(addr + 1);

                if (Machine.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                    addr += 2;
                else
                    addr += 1;
            }

            VariableManager.StoreWord(dest, addr);
        }
    }
}