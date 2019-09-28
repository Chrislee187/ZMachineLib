using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class StoreB : ZMachineOperationBase
    {
        public StoreB(IZMemory memory)
            : base((ushort)OpCodes.StoreB, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var addr = (ushort)(args[0] + args[1]);
            Contents.Manager.Set(addr, (byte)args[2]);
        }
    }
}