using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// Jump if object a is a direct child of b, i.e., if parent of a is b.
    /// </summary>
    public sealed class Jin : ZMachineOperationBase
    {
        public Jin(IZMemory contents)
            : base((ushort)OpCodes.Jin, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var zObj = Contents.ObjectTree[operands[0]]; // ObjectManager.GetObject(operands[0]);

            Jump(zObj.Parent == operands[1]);
        }
    }
}