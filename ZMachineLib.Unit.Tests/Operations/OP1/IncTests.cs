using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:133 5 inc (variable)
    /// Increment variable by 1. (This is signed, so -1 increments to 0.)
    /// </summary>
    public class IncTests : OperationsTestsBase<Inc>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((short)10, (short)11)]
        [TestCase((short)-1, (short)0)]
        [TestCase((short)-10, (short)-9)]
        public void Should_store_Inc_result(short value, short expected)
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