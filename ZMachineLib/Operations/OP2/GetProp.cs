using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OP2
{
    public sealed class GetProp : ZMachineOperation
    {
        public GetProp(ZMachine2 machine)
            : base((ushort)OpCodes.GetProp, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Log.Write($"[{ObjectManager.GetObjectName(operands[0])}] ");

            var dest = PeekNextByte();
            ushort val = 0;

            byte prop = (byte)operands[1];
            var addr = ObjectManager.GetPropertyAddress(operands[0], prop);
            if (addr > 0)
            {
                var propInfo = Machine.Memory[addr++];
                byte len;

                if (Machine.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                    len = (byte)(Machine.Memory[addr++] & 0x3f);
                else
                    len = (byte)((propInfo >> ((ushort) Machine.Header.Version <= 3 ? 5 : 6)) + 1);

                for (var i = 0; i < len; i++)
                    val |= (ushort)(Machine.Memory[addr + i] << (len - 1 - i) * 8);
            }
            else
                val = Machine.Memory.GetUshort((ushort)(Machine.Header.ObjectTable + (operands[1] - 1) * 2));

            VariableManager.StoreWord(dest, val);
        }
    }
}