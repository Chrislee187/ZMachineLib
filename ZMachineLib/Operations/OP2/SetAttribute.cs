using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OP2
{
    public sealed class SetAttribute : ZMachineOperation
    {
        public SetAttribute(ZMachine2 machine)
            : base((ushort)OpCodes.SetAttribute, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var obj = operands[0];
            var attr = operands[1];

            if (obj == 0)
                return;

            var zObj = ObjectManager.GetObject(obj);
            zObj.SetAttribute(attr);
        }
    }
}