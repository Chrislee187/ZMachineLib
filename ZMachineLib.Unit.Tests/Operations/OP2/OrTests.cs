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
            base.Setup();
            _op = new Or(ZMachine2, VariableManagerMockery.Object);
            MockGetNextByte(_op);
        }

        [TestCase((ushort) 0x01, (ushort) 0x02 )]
        public void Should_store_bitwise_OR_result(ushort val1, ushort val2)
        {
            var args = new OpArgBuilder()
                .WithValue(val1)
                .WithValue(val2)
                .Build();

            // TODO: Need to be able to Mock Memory & Stack interactions
            _op.Execute(args);

            var expectedValue = (ushort) (val1 | val2);
            VariableManagerMockery.VerifyStoreWord(expectedValue);

        }

    }
}