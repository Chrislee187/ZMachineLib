using NUnit.Framework;
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
            base.Setup();
            _op = new Jin(ZMachine2, objectManager: ObjectManagerMocks.Object);
            MockJump(_op);
        }

        [Test]
        public void Should_jump_if_objectA_is_child_of_objectB()
        {
            ushort parent = 20;

            var args = new OpArgBuilder()
                .WithValue(AnyValue)
                .WithValue(parent)
                .Build();

            ObjectManagerMocks
                .SetupGetObjectAddress(AnyValue)
                .SetupGetObjectParent(parent);

            _op.Execute(args);

            JumpedWith(true);
        }

        [Test]
        public void Should_not_jump_if_objectA_is_NOT_child_of_objectB()
        {
            ushort parent = 20;
            ushort notParent = 30;

            var args = new OpArgBuilder()
                .WithValue(AnyValue)
                .WithValue(parent)
                .Build();

            ObjectManagerMocks
                .SetupGetObjectAddress(AnyValue)
                .SetupGetObjectParent(notParent);

            _op.Execute(args);

            JumpedWith(false);
        }

    }

    public class TestTests : OperationsTestsBase
    {
        private Test _op;


        [SetUp]
        public void SetUp()
        {
            base.Setup();
            _op = new Test(ZMachine2);
            MockJump(_op);
        }

        [Test]
        public void Should_jump_if_all_flags_set()
        {
            ushort bitmap = 0xFF;
            ushort flags = 0x0F;

            var args = new OpArgBuilder()
                .WithValue(bitmap)
                .WithValue(flags)
                .Build();

            _op.Execute(args);

            JumpedWith(true);
        }
        [Test]
        public void Should_NOT_jump_if_any_flags_NOT_set()
        {
            ushort bitmap = 0x0E;
            ushort flags = 0x0F;

            var args = new OpArgBuilder()
                .WithValue(bitmap)
                .WithValue(flags)
                .Build();

            _op.Execute(args);

            JumpedWith(false);
        }

    }

}