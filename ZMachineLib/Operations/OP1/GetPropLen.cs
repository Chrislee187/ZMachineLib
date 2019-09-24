using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class GetPropLen : ZMachineOperationBase
    {
        public GetPropLen(ZMachine2 machine)
            : base((ushort)OpCodes.GetPropLen, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var dest = GetNextByte();
            var propInfo = MemoryManager.Get(operands[0] - 1);
            byte len;
            if (Machine.Contents.Header.Version > 3 && (propInfo & 0x80) == 0x80)
            {
                len = (byte) (MemoryManager.Get(operands[0] - 1) & 0x3f);
                if (len == 0)
                    len = 64;
            }
            else
                len = (byte)((propInfo >> ((ushort) Machine.Contents.Header.Version <= 3 ? 5 : 6)) + 1);

            Contents.VariableManager.StoreByte(dest, len);
        }
    }
}