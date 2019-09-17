using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OP2
{
    public sealed class ClearAttribute : ZMachineOperation
    {
        public ClearAttribute(ZMachine2 machine)
            : base((ushort)OpCodes.ClearAttribute, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{ObjectManager.GetObjectName(args[0])}] ");

            var objectAddr = ObjectManager.GetObjectAddress(args[0]);
            ulong attributes;
            ulong flag;

            if (Machine.Header.Version <= 3)
            {
                attributes = Machine.Memory.GetUInt(objectAddr);
                flag = 0x80000000 >> args[1];
                attributes &= ~flag;
                uint val = (uint)attributes;
                Machine.Memory.StoreAt(objectAddr, val);
            }
            else
            {
                attributes = (ulong)Machine.Memory.GetUInt(objectAddr) << 16 | Machine.Memory.GetUshort((uint)(objectAddr + 4));
                flag = (ulong)(0x800000000000 >> args[1]);
                attributes &= ~flag;
                uint val = (uint)attributes >> 16;
                Machine.Memory.StoreAt(objectAddr, val);
                ushort value = (ushort)attributes;
                Machine.Memory.StoreAt((ushort)(objectAddr + 4), value);
            }
        }
    }
}