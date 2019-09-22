using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class OrTests : OperationsTestsBase
    {
        private Or _op;


        [SetUp]
        public void SetUp()
        {
            Setup();
            _op = new Or(ZMachine2, MemoryMock);
            MockPeekNextByte(_op);
        }

        [TestCase((ushort) 0x01, (ushort) 0x02 )]
        public void Should_store_bitwise_OR_result(ushort val1, ushort val2)
        {
            var args = new OpArgBuilder()
                .WithValue(val1)
                .WithValue(val2)
                .Build();

            _op.Execute(args);

            var expectedValue = (ushort) (val1 | val2);
            VariableManagerMockery.VerifyStoreWord(expectedValue);

        }

    }
}