using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class SetAttribute : ZMachineOperationBase
    {
        public SetAttribute(IZMemory contents)
            : base((ushort)OpCodes.SetAttribute, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var attr = args[1];

            if (obj == 0)
                return;

            Contents.ObjectTree[obj].SetAttribute(attr);
        }
    }
}