using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class ClearAttribute : ZMachineOperation
    {
        public ClearAttribute(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.ClearAttribute, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            var objectAddr = GetObjectAddress(args[0]);
            ulong attributes;
            ulong flag;

            if (Version <= 3)
            {
                attributes = GetUint(objectAddr);
                flag = 0x80000000 >> args[1];
                attributes &= ~flag;
                StoreUint(objectAddr, (uint)attributes);
            }
            else
            {
                attributes = (ulong)GetUint(objectAddr) << 16 | GetWord((uint)(objectAddr + 4));
                flag = (ulong)(0x800000000000 >> args[1]);
                attributes &= ~flag;
                StoreUint(objectAddr, (uint)attributes >> 16);
                StoreWord((ushort)(objectAddr + 4), (ushort)attributes);
            }
        }
    }
}