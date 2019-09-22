using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class SubTests : OperationsTestsBase
    {
        private Sub _op;


        [SetUp]
        public void SetUp()
        {
            Setup();
            _op = new Sub(ZMachine2, MemoryMock);
            MockPeekNextByte(_op);
        }

        [TestCase((short)1, (short)2)]
        [TestCase((short)-1, (short)-1)]
        public void Should_store_AND_result(short val1, short val2)
        {
            var args = new OpArgBuilder()
                .WithValue((ushort)val1)
                .WithValue((ushort)val2)
                .Build();

            _op.Execute(args);

            VariableManagerMockery
                .VerifyStoreWord((ushort)(val1 - val2));
        }
    }
}