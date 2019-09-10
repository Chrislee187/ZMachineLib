using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public sealed class TestAttribute : ZMachineOperation
    {
        public TestAttribute(ZMachine2 machine)
            : base((ushort)Kind2OpCodes.TestAttribute, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var attr = args[1];

            Log.Write($"[{GetObjectName(obj)}] ");
            PrintObjectInfo(obj, false);

            var objectAddr = GetObjectAddress(obj);
            ulong attributes;
            ulong flag;

            if (Version <= 3)
            {
                attributes = GetUint(objectAddr);
                flag = 0x80000000 >> attr;
            }
            else
            {
                attributes = (ulong)GetUint(objectAddr) << 16 | GetWord((uint)(objectAddr + 4));
                flag = (ulong)(0x800000000000 >> attr);
            }

            var branch = (flag & attributes) == flag;
            Jump(branch);
        }
    }
}