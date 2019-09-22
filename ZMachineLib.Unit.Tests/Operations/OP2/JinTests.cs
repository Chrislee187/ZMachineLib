using Moq;
using NUnit.Framework;
using ZMachineLib.Content;
using ZMachineLib.Operations.OP2;
using TestAttribute = NUnit.Framework.TestAttribute;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// Jump if object a is a direct child of b, i.e., if parent of a is b.
    /// </summary>
    public class JinTests : OperationsTestsBase
    {
        private Jin _op;

        [SetUp]
        public void SetUp()
        {
            Setup();
            _op = new Jin(MemoryMock);
            MockJump(_op);
        }

        [Test]
        public void Should_jump_if_objectA_is_child_of_objectB()
        {
            ushort parent = 1234;

            // TODO: Create an ObjectTreeMockery for this;
            _objectTreeMock
                .Setup(m => m[It.IsAny<ushort>()])
                .Returns(new ZMachineObjectBuilder().WithParent(parent).Build());

            var operands = new OpArgBuilder()
                .WithAnyValue()
                .WithValue(parent)
                .Build();
            _op.Execute(operands);

            JumpedWith(true);
        }

        [Test]
        public void Should_not_jump_if_objectA_is_NOT_child_of_objectB()
        {
            const ushort parent = 1234;
            const ushort notParent = 4321;

            // TODO: Create an ObjectTreeMockery for this;
            _objectTreeMock
                .Setup(m => m[It.IsAny<ushort>()])
                .Returns(new ZMachineObjectBuilder().WithParent(parent).Build());

            _op.Execute(new OpArgBuilder()
                .WithValues(parent, notParent)
                .Build());

            JumpedWith(false);
        }

    }
}