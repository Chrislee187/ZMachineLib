using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:8 8 or a b -> (result)
    /// Bitwise OR.
    /// </summary>
    public class OrTests : OperationsTestsBase<Or>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((ushort) 0x01, (ushort) 0x02 )]
        public void Should_store_bitwise_OR_result(ushort val1, ushort val2)
        {
            var args = new OperandBuilder()
                .WithArg(val1)
                .WithArg(val2)
                .Build();

            Operation.Execute(args);

            var expectedValue = (ushort) (val1 | val2);
            VariableManagerMockery.UShortWasStored(expectedValue);

        }

    }
}