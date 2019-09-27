using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class SubTests : OperationsTestsBase<Sub>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((short)1, (short)2)]
        [TestCase((short)-1, (short)-1)]
        public void Should_store_AND_result(short val1, short val2)
        {
            var args = new OpArgBuilder()
                .WithValue((ushort)val1)
                .WithValue((ushort)val2)
                .Build();

            Operation.Execute(args);

            VariableManagerMockery
                .UShortWasStored((ushort)(val1 - val2));
        }
    }
}