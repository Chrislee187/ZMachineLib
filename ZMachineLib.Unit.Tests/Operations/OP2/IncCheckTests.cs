using NUnit.Framework;
using ZMachineLib.Operations.OP2;
using TestAttribute = NUnit.Framework.TestAttribute;
// ReSharper disable PossibleInvalidOperationException

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class IncCheckTests : OperationsTestsBase
    {
        private IncCheck _op;


        [SetUp]
        public void SetUp()
        {
            Setup();
            _op = new IncCheck(ZMachine2, MemoryMock, objectManager: ObjectManagerMockery.Object);
            MockJump(_op);
        }

        [Test]
        public void Should_increment_value_and_jump_if_greater()
        {
            ushort initialValue = 10;
            ushort comparison = 10;
            var args = new OpArgBuilder()
                .WithValue(AnyValue)
                .WithValue(comparison)
                .Build();
            
            VariableManagerMockery.SetupGetWord(initialValue);

            _op.Execute(args);

            var expectedValue = (initialValue + 1);

            VariableManagerMockery.VerifyStoreWord((ushort) expectedValue);

            JumpedWith(true);
        }

        [Test]
        public void Should_increment_value_and_NOT_jump()
        {
            ushort initialValue = 10;
            ushort comparison = 15;
            var args = new OpArgBuilder()
                .WithValue(AnyValue)
                .WithValue(comparison)
                .Build();

            VariableManagerMockery.SetupGetWord(initialValue);

            _op.Execute(args);

            var expectedValue = (initialValue + 1);

            VariableManagerMockery.VerifyStoreWord((ushort) expectedValue);

            JumpedWith(false);

        }
    }
}