using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:142 E load(variable) -> (result)
    /// The value of the variable referred to by the operand is stored in the result.
    /// (Inform doesn't use this; see the notes to S 14.)
    /// </summary>
    public class LoadTests : OperationsTestsBase<Load>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_retrieve_variable_and_store()
        {
            const ushort value = 1234;
            Mockery
                .VariableRetrieves(value);

            var args = new OperandBuilder()
                .WithArg(AnyVariable)
                .Build();

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStored(value);
        }
    }
}