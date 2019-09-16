using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OP2
{
    public sealed class TestAttribute : ZMachineOperation
    {
        public TestAttribute(ZMachine2 machine)
            : base((ushort)OpCodes.TestAttribute, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var attr = args[1];

            Log.Write($"[{ObjectManager.GetObjectName(obj)}] ");
            ObjectManager.PrintObjectInfo(obj, false);

            var objectAddr = ObjectManager.GetObjectAddress(obj);
            ulong attributes;
            ulong flag;

            if (Version <= 3)
            {
                attributes = Memory.GetUInt(objectAddr);
                flag = 0x80000000 >> attr;
            }
            else
            {
                attributes = (ulong)Memory.GetUInt(objectAddr) << 16 | Machine.Memory.GetUshort((uint)(objectAddr + 4));
                flag = (ulong)(0x800000000000 >> attr);
            }

            var branch = (flag & attributes) == flag;
            Jump(branch);
        }
    }
}