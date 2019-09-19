using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class LoadBTests : OperationsTestsBase
    {
        private LoadB _op;


        [SetUp]
        public void SetUp()
        {
            base.Setup();
            _op = new LoadB(ZMachine2, VariableManagerMockery.Object);
            MockPeekNextByte(_op);
        }

        [TestCase((ushort)0x01, (ushort)0x02)]
        public void Should_store_the_value_from_addr_plus_index(ushort addr, ushort index)
        {
            var args = new OpArgBuilder()
                .WithValue(addr)
                .WithValue(index)
                .Build();

            // TODO: Need a "MemoryManager" so we can mock/expect operations directly on the byte-array
            _op.Execute(args);

            var expectedValue = (byte) 0;
            VariableManagerMockery.VerifyStoreByte(expectedValue);

        }

    }
}