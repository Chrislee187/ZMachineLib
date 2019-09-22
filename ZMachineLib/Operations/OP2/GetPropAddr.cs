using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    public sealed class GetPropAddr : ZMachineOperationBase
    {
        public GetPropAddr(ZMachine2 machine)
            : base((ushort)OpCodes.GetPropAddr, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetNextByte();
            byte prop = (byte)operands[1];
            var addr = ObjectManager.GetPropertyAddress(operands[0], prop);

            if (addr > 0)
            {
                var propInfo = MemoryManager.Get(addr + 1);

                if (Machine.Contents.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                    addr += 2;
                else
                    addr += 1;
            }

            Contents.VariableManager.StoreWord(dest, addr);
        }
    }
}