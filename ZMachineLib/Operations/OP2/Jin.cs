using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Jump if object a is a direct child of b, i.e., if parent of a is b.
    /// </summary>
    public sealed class Jin : ZMachineOperationBase
    {
        public Jin(ZMachine2 machine,
            IZMemory contents,
            IObjectManager objectManager = null)
            : base((ushort)OpCodes.Jin, machine, contents, objectManager)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = ObjectManager.GetObject(operands[0]);

            Jump(zObj.Parent == operands[1]);
        }
    }
}