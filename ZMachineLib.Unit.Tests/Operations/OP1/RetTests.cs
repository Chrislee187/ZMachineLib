using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:139 B ret value
    /// Returns from the current routine with the value given.
    /// </summary>
    public class RetTests : OperationsTestsBase<Ret>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_store_return_value_when_asked()
        {
            Mockery
                .StartingPC(100, true);

            var args = new OperandBuilder()
                .WithArg(AnyValue)
                .Build();

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStored(AnyValue);
        }


        [Test]
        public void Should_not_store_when_routine_does_not_store()
        {
            Mockery
                .StartingPC(100);

            var args = new OperandBuilder()
                .WithArg(AnyValue)
                .Build();

            Operation.Execute(args);

            Mockery
                .NoResultDestinationRetrieved()
                .NoResultWasStored();
        }
    }
}