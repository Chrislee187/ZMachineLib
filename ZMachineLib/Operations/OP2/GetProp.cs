using System.Collections.Generic;
using ZMachineLib.Extensions;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:17 11 get_prop object property -> (result)
    /// Read property from object (resulting in the default value if it had no such
    /// declared property).
    /// If the property has length 1, the value is only that byte.
    /// If it has length 2, the first two bytes of the property are taken as a word value.
    /// It is illegal for the opcode to be used if the property has length greater than 2,
    /// and the result is unspecified.
    /// </summary>
    public sealed class GetProp : ZMachineOperationBase
    {
        public GetProp(ZMachine2 machine)
            : base((ushort)OpCodes.GetProp, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetNextByte();
            ushort val = 0;

            byte prop = (byte)operands[1];
            var addr = ObjectManager.GetPropertyAddress(operands[0], prop);
            if (addr > 0)
            {
                var propInfo = MemoryManager.Get(addr++);
                byte len;

                if (Machine.Contents.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                    len = (byte)(MemoryManager.Get(addr++) & 0x3f);
                else
                    len = (byte)((propInfo >> ((ushort) Machine.Contents.Header.Version <= 3 ? 5 : 6)) + 1);

                for (var i = 0; i < len; i++)
                    val |= (ushort)(MemoryManager.Get(addr + i) << (len - 1 - i) * 8);
            }
            else
                val = Machine.Memory.GetUShort((ushort)(Machine.Contents.Header.ObjectTable + (operands[1] - 1) * 2));

            Contents.VariableManager.StoreWord(dest, val);
        }
    }
}