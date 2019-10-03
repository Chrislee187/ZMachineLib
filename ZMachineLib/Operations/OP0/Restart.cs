using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public class Restart : ZMachineOperationBase
    {
        public Restart(IZMemory memory)
            : base((ushort)OpCodes.Restart, memory)
        {
        }
        public override void Execute(List<ushort> args)
        {
            Memory.Restart();
        }

    }
}