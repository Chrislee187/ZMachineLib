using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP1
{
    /// <summary>
    /// 1OP:128 0 jz a ?(label)
    /// Jump if a = 0.
    /// </summary>
    public sealed class Jz : ZMachineOperationBase
    {
        public Jz(IZMemory contents)
            : base((ushort)OpCodes.Jz, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            // TODO: Refactor the underlying jump code before sorting the test out for this
            Contents.Jump(args[0] == 0);
        }
    }
}