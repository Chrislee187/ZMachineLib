using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class ScanTable : ZMachineOperationBase
    {
        public ScanTable(IZMemory memory)
            : base((ushort)OpCodes.ScanTable, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = Contents.GetCurrentByteAndInc();
            byte len = 0x02;

            if (operands.Count == 4)
                len = (byte)(operands[3] & 0x7f);

            for (var i = 0; i < operands[2]; i++)
            {
                var addr = (ushort)(operands[1] + i * len);
                ushort val;

                if (operands.Count == 3 || (operands[3] & 0x80) == 0x80)
                    val = Contents.Manager.GetUShort(addr);
                else
                    val = Contents.Manager.Get(addr);

                if (val == operands[0])
                {
                    Contents.VariableManager.StoreWord(dest, addr);
                    Jump(true);
                    return;
                }
            }

            Contents.VariableManager.StoreWord(dest, 0);
            Jump(false);
        }
    }
}