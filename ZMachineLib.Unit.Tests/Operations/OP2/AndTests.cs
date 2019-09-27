using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:9 9 and a b -> (result)
    /// Bitwise AND.
    /// </summary>
    public class AndTests : OperationsTestsBase<And>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((ushort)0x01, (ushort)0x02)]
        public void Should_store_bitwise_AND_result(ushort val1, ushort val2)
        {
            var args = new OpArgBuilder()
                .WithValue(val1)
                .WithValue(val2)
                .Build();

            // TODO: Need to be able to Mock Memory & Stack interactions
            Operation.Execute(args);

            var expectedValue = (ushort)(val1 & val2);
            VariableManagerMockery
                .UShortWasStored(expectedValue);

        }

    }
}