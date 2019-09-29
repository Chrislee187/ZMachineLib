using NUnit.Framework;
using ZMachineLib.Operations.OP1;


namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:134 6 dec (variable)
    /// Decrement variable by 1. This is signed, so 0 decrements to -1.
    /// </summary>
    public class DecTests : OperationsTestsBase<Dec>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((short)10, (short)9)]
        [TestCase((short)0, (short)-1)]
        [TestCase((short)-10, (short)-11)]
        public void Should_store_Dec_result(short value, short expected)
        {
            Mockery
                .VariableRetrieves(value);

            var args = new OperandBuilder()
                .WithArg(AnyVariable)
                .Build();

            Operation.Execute(args);

            Mockery
                .ResultStored(expected);
        }
    }
}