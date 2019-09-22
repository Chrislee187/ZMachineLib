using NUnit.Framework;
using ZMachineLib.Operations.OP2;
using TestAttribute = NUnit.Framework.TestAttribute;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class DecCheckTests : OperationsTestsBase
    {
        private DecCheck _op;

        [SetUp]
        public void SetUp()
        {
            Setup();
            _op = new DecCheck(MemoryMock);
            MockJump(_op);
        }

        [Test]
        public void Should_decrement_value_and_jump()
        {
            ushort initialValue = 10;
            ushort comparison = 10;
            var args = new OpArgBuilder()
                .WithValue(AnyValue)
                .WithValue(comparison)
                .Build();

            VariableManagerMockery.SetupGetWord(initialValue);

            _op.Execute(args);

            var expectedValue = initialValue - 1;

            VariableManagerMockery.VerifyStoreWord((ushort)expectedValue);

            JumpedWith(true);
        }

        [Test]
        public void Should_decrement_value_and_NOT_jump()
        {
            ushort initialValue = 10;
            ushort comparison = 5;
            var args = new OpArgBuilder()
                .WithValue(AnyValue)
                .WithValue(comparison)
                .Build();

            VariableManagerMockery.SetupGetWord(initialValue);

            _op.Execute(args);

            var expectedValue = initialValue - 1;

            VariableManagerMockery.VerifyStoreWord((ushort)expectedValue);

            JumpedWith(false);
        }
    }
}