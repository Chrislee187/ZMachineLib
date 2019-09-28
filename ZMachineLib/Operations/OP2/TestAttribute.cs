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
            : base((ushort)OpCodes.TestAttribute, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var obj = args[0];
            var attr = args[1];
            var zObj = Contents.ObjectTree[obj];

            Jump(zObj.TestAttribute(attr));
        }
    }

}