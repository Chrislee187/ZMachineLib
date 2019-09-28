using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class StoreW : ZMachineOperationBase
    {
        public StoreW(IZMemory memory)
            : base((ushort)OpCodes.StoreW, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + 2 * args[1]);
            ushort value = args[2];
            Contents.Manager.SetUShort(addr, value);
        }
    }
}