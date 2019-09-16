using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class ScanTable : ZMachineOperation
    {
        public ScanTable(ZMachine2 machine)
            : base((ushort)OpCodes.ScanTable, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory[Stack.Peek().PC++];
            byte len = 0x02;

            if (args.Count == 4)
                len = (byte)(args[3] & 0x7f);

            for (var i = 0; i < args[2]; i++)
            {
                var addr = (ushort)(args[1] + i * len);
                ushort val;

                if (args.Count == 3 || (args[3] & 0x80) == 0x80)
                    val = Machine.Memory.GetUshort(addr);
                else
                    val = Memory[addr];

                if (val == args[0])
                {
                    StoreWordInVariable(dest, addr);
                    Jump(true);
                    return;
                }
            }

            StoreWordInVariable(dest, 0);
            Jump(false);
        }
    }
}