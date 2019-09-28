using NUnit.Framework;
using ZMachineLib.Operations.OP2;
using TestAttribute = NUnit.Framework.TestAttribute;
// ReSharper disable PossibleInvalidOperationException

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:5 5 inc_chk (variable) value ?(label)
    /// Increment variable, and branch if now greater than value.
    /// </summary>
    public class IncCheckTests : OperationsTestsBase<IncCheck>
    {

        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_increment_value_and_jump_if_greater()
        {
            ushort initialValue = 10;
            ushort comparison = 10;
            var args = new OperandBuilder()
                .WithArg(AnyValue)
                .WithArg(comparison)
                .Build();

            Mockery.VariableRetrieves(initialValue);

            Operation.Execute(args);

            var expectedValue = (initialValue + 1);

            Mockery
                .ResultStored((ushort) expectedValue)
                .JumpedWith(true);
        }

        [Test]
        public void Should_increment_value_and_NOT_jump()
        {
            ushort initialValue = 10;
            ushort comparison = 15;
            var args = new OperandBuilder()
                .WithArg(AnyValue)
                .WithArg(comparison)
                .Build();

            Mockery.VariableRetrieves(initialValue);

            Operation.Execute(args);

            var expectedValue = (initialValue + 1);

            Mockery
                .ResultStored((ushort)expectedValue)
                .JumpedWith(false);

        }
    }
}