using NUnit.Framework;
using ZMachineLib.Operations.OP2;
using TestAttribute = NUnit.Framework.TestAttribute;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:6 6 jin obj1 obj2 ?(label)
    /// Jump if object a is a direct child of b, i.e., if parent of a is b.
    /// </summary>
    public class JinTests : OperationsTestsBase<Jin>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_jump_if_objectA_is_child_of_objectB()
        {
            ushort parent = 1234;

            Mockery.SetNextObject(
                new ZMachineObjectBuilder()
                .WithParent(parent)
                .Build()
                );

            var operands = new OperandBuilder()
                .WithAnyArg()
                .WithArg(parent)
                .Build();

            Operation.Execute(operands);

            Mockery.JumpedWith(true);
        }

        [Test]
        public void Should_not_jump_if_objectA_is_NOT_child_of_objectB()
        {
            const ushort parent = 1234;
            const ushort notParent = 4321;

            Mockery.SetNextObject(
                new ZMachineObjectBuilder()
                .WithParent(parent)
                .Build()
                );

            Operation.Execute(new OperandBuilder()
                .WithArgs(parent, notParent)
                .Build());

            Mockery.JumpedWith(false);
        }

    }
}