using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OP2
{
    /// <summary>
    /// 2OP:10 A test_attr object attribute ?(label)
    /// Jump if object has attribute
    /// </summary>
    public sealed class TestAttribute : ZMachineOperation
    {
        public TestAttribute(ZMachine2 machine)
            : base((ushort)OpCodes.TestAttribute, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var obj = operands[0];
            var attr = operands[1];

            Log.Write($"[{ObjectManager.GetObjectName(obj)}] ");

            var zObj = ObjectManager.GetObject(obj);

            Jump(zObj.TestAttribute(attr));
        }
    }

}