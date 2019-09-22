using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class SetAttribute : ZMachineOperationBase
    {
        public SetAttribute(ZMachine2 machine, IZMemory contents)
            : base((ushort)OpCodes.SetAttribute, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var obj = operands[0];
            var attr = operands[1];

            if (obj == 0)
                return;

            var zObj = ObjectManager.GetObject(obj);
//            var zObj = Contents.ObjectTree[obj];
            zObj.SetAttribute(attr);
        }
    }
}