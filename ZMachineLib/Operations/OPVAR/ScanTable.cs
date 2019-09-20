using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class ScanTable : ZMachineOperation
    {
        public ScanTable(ZMachine2 machine)
            : base((ushort)OpCodes.ScanTable, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = PeekNextByte();
            byte len = 0x02;

            if (operands.Count == 4)
                len = (byte)(operands[3] & 0x7f);

            for (var i = 0; i < operands[2]; i++)
            {
                var addr = (ushort)(operands[1] + i * len);
                ushort val;

                if (operands.Count == 3 || (operands[3] & 0x80) == 0x80)
                    val = Machine.Memory.GetUShort(addr);
                else
                    val = MemoryManager.Get(addr);

                if (val == operands[0])
                {
                    VariableManager.StoreWord(dest, addr);
                    Jump(true);
                    return;
                }
            }

            VariableManager.StoreWord(dest, 0);
            Jump(false);
        }
    }
}