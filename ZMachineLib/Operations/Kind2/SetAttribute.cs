using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class SetAttribute : ZMachineOperation
    {
        public SetAttribute(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.SetAttribute, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var attr = args[1];

            if (obj == 0)
                return;

            Log.Write($"[{GetObjectName(obj)}] ");

            var objectAddr = GetObjectAddress(obj);
            ulong attributes;
            ulong flag;

            if (Version <= 3)
            {
                attributes = GetUint(objectAddr);
                flag = 0x80000000 >> attr;
                attributes |= flag;
                StoreUint(objectAddr, (uint)attributes);
            }
            else
            {
                attributes = (ulong)GetUint(objectAddr) << 16 | GetWord((uint)(objectAddr + 4));
                flag = (ulong)(0x800000000000 >> attr);
                attributes |= flag;
                StoreUint(objectAddr, (uint)(attributes >> 16));
                StoreWord((ushort)(objectAddr + 4), (ushort)attributes);
            }
        }
    }
}