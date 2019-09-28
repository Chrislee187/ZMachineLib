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

            ObjectTreeMockery.SetupGetIndexerReturns(
                new ZMachineObjectBuilder()
                    .WithParent(parent)
                    .Build()
            );

            var operands = new OpArgBuilder()
                .WithAnyValue()
                .WithValue(parent)
                .Build();

            Operation.Execute(operands);

            JumpedWith(true);
        }

        [Test]
        public void Should_not_jump_if_objectA_is_NOT_child_of_objectB()
        {
            const ushort parent = 1234;
            const ushort notParent = 4321;
            ObjectTreeMockery.SetupGetIndexerReturns(
                new ZMachineObjectBuilder()
                    .WithParent(parent)
                    .Build()
            );
            // TODO: Create an ObjectTreeMockery for this;
            Operation.Execute(new OpArgBuilder()
                .WithValues(parent, notParent)
                .Build());

            JumpedWith(false);
        }

    }
}