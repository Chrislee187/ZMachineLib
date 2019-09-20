using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class PutProp : ZMachineOperation
    {
        public PutProp(ZMachine2 machine)
            : base((ushort)OpCodes.PutProp, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var prop = ObjectManager.GetPropertyHeaderAddress(operands[0]);
            var size = MemoryManager.Get(prop);
            prop += (ushort)(size * 2 + 1);

            while (MemoryManager.Get(prop) != 0x00)
            {
                var propInfo = MemoryManager.Get(prop++);
                byte len;
                if (Machine.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte)(MemoryManager.Get(prop++) & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte)((propInfo >> ((ushort) Machine.Header.Version <= 3 ? 5 : 6)) + 1);

                var propNum = (byte)(propInfo & ((ushort) Machine.Header.Version <= 3 ? 0x1f : 0x3f));
                if (propNum == operands[1])
                {
                    if (len == 1)
                        MemoryManager.Set(prop + 1, (byte)operands[2]);
                    else
                    {
                        ushort value = operands[2];
                        MemoryManager.Set(prop, value);
                    }

                    break;
                }

                prop += len;
            }
        }
    }
}