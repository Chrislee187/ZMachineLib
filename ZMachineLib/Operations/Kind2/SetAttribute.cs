using System.Collections.Generic;
using ZMachineLib.Extensions;

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
                attributes = Memory.GetUInt(objectAddr);
                flag = 0x80000000 >> attr;
                attributes |= flag;
                uint val = (uint)attributes;
                Memory.StoreAt(objectAddr, val);
            }
            else
            {
                attributes = (ulong)Memory.GetUInt(objectAddr) << 16 | Machine.Memory.GetUshort((uint)(objectAddr + 4));
                flag = (ulong)(0x800000000000 >> attr);
                attributes |= flag;
                uint val = (uint)(attributes >> 16);
                Memory.StoreAt(objectAddr, val);
                ushort value = (ushort)attributes;
                Memory.StoreAt((ushort)(objectAddr + 4), value);
            }
        }
    }
}