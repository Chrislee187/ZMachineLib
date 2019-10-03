using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:12 C clear_attr object attribute
    /// Make object not have the attribute numbered attribute
    /// </summary>
    public sealed class ClearAttribute : ZMachineOperationBase
    {
        public ClearAttribute(IZMemory memory)
            : base((ushort)OpCodes.ClearAttribute, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Memory.ObjectTree.GetOrDefault(args[0])
                .ClearAttribute(args[1]);
        }
    }
}