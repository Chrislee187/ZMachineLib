using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP0
{
    public sealed class Verify : ZMachineOperationBase
    {
        public Verify(IZMemory memory)
            : base((ushort)OpCodes.Verify, memory)
        {}

        public override void Execute(List<ushort> args) => Contents.Jump(true);
    }
}