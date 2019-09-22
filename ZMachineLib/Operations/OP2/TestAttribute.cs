using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:10 A test_attr object attribute ?(label)
    /// Jump if object has attribute
    /// </summary>
    public sealed class TestAttribute : ZMachineOperationBase
    {
        public TestAttribute(ZMachine2 machine)
            : base((ushort)OpCodes.TestAttribute, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var obj = operands[0];
            var attr = operands[1];
            var zObj = ObjectManager.GetObject(obj);

            Jump(zObj.TestAttribute(attr));
        }
    }

}