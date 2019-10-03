using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class ScanTable : ZMachineOperationBase
    {
        public ScanTable(IZMemory memory)
            : base((ushort)OpCodes.ScanTable, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory.GetCurrentByteAndInc();
            byte len = 0x02;

            if (args.Count == 4)
                len = (byte)(args[3] & 0x7f);

            for (var i = 0; i < args[2]; i++)
            {
                var addr = (ushort)(args[1] + i * len);
                ushort val;

                if (args.Count == 3 || (args[3] & 0x80) == 0x80)
                    val = Memory.Manager.GetUShort(addr);
                else
                    val = Memory.Manager.Get(addr);

                if (val == args[0])
                {
                    Memory.VariableManager.Store(dest, addr);
                    Memory.Jump(true);
                    return;
                }
            }

            Memory.VariableManager.Store(dest, 0);
            Memory.Jump(false);
        }
    }
}