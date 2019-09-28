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
            var dest = Contents.GetCurrentByteAndInc();
            byte len = 0x02;

            if (args.Count == 4)
                len = (byte)(args[3] & 0x7f);

            for (var i = 0; i < args[2]; i++)
            {
                var addr = (ushort)(args[1] + i * len);
                ushort val;

                if (args.Count == 3 || (args[3] & 0x80) == 0x80)
                    val = Contents.Manager.GetUShort(addr);
                else
                    val = Contents.Manager.Get(addr);

                if (val == args[0])
                {
                    Contents.VariableManager.Store(dest, addr);
                    Contents.Jump(true);
                    return;
                }
            }

            Contents.VariableManager.Store(dest, 0);
            Contents.Jump(false);
        }
    }
}