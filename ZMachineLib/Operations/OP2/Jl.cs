using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:2 2 jl a b ?(label)
    /// <![CDATA[
    /// /// Jump if a < b (using a signed 16-bit comparison).
    /// ]]>
    /// </summary>
    public sealed class Jl : ZMachineOperationBase
    {
        public Jl(IZMemory contents)
            : base((ushort)OpCodes.Jl, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Jump((short)args[0] < (short)args[1]);
        }
    }
}