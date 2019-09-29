using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    public class NotTests : OperationsTestsBase<Not>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((ushort) 0x0001, (ushort) 0xFFFE)]
        [TestCase((ushort) 0xFFFE, (ushort) 0x0001)]
        [TestCase((ushort) 0b1010_1010_1010_1010, (ushort) 0b0101_0101_0101_0101)]
        public void Should_store_Dec_result(ushort value, ushort expected)
        {
            Mockery
                .VariableRetrieves(value);

            var args = new OperandBuilder()
                .WithArg(value)
                .Build();

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStored(expected);
        }
    }
}