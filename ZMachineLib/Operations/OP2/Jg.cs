using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:3 3 jg a b ?(label)
    /// Jump if a > b(using a signed 16-bit comparison).
    /// </summary>
    public sealed class Jg : ZMachineOperationBase
    {
        public Jg(IZMemory memory)
            : base((ushort)OpCodes.Jg, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Memory.Jump((short)args[0] > (short)args[1]);
        }
    }
}