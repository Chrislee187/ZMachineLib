using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class SetAttribute : ZMachineOperationBase
    {
        public SetAttribute(IZMemory memory)
            : base((ushort)OpCodes.SetAttribute, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var attr = args[1];

            if (obj == 0)
                return;

            Memory.ObjectTree[obj].SetAttribute(attr);
        }
    }
}