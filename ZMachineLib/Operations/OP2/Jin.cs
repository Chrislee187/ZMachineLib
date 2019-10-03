using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:6 6 jin obj1 obj2 ?(label)
    /// Jump if object a is a direct child of b, i.e., if parent of a is b.
    /// </summary>
    public sealed class Jin : ZMachineOperationBase
    {
        public Jin(IZMemory memory)
            : base((ushort)OpCodes.Jin, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var zObj = Memory.ObjectTree.GetOrDefault(args[0]);

            Memory.Jump(zObj.Parent == args[1]);
        }
    }
}