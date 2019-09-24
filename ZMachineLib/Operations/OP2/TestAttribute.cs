using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:10 A test_attr object attribute ?(label)
    /// Jump if object has attribute
    /// </summary>
    public sealed class TestAttribute : ZMachineOperationBase
    {
        public TestAttribute(IZMemory contents)
            : base((ushort)OpCodes.TestAttribute, null, contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var obj = operands[0];
            var attr = operands[1];
            var zObj = Contents.ObjectTree[obj];

            Jump(zObj.TestAttribute(attr));
        }
    }

}