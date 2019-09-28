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

        [TestCase((ushort) 0b0, (ushort) 0b0, (ushort) 0b0 )]
        [TestCase((ushort) 0b0, (ushort) 0b1, (ushort) 0b1 )]
        [TestCase((ushort) 0b1, (ushort) 0b0, (ushort) 0b1 )]
        [TestCase((ushort) 0b1, (ushort) 0b1, (ushort) 0b1 )]
        public void Should_store_bitwise_OR_result(ushort val1, ushort val2, ushort expected)
        {
            var args = new OperandBuilder()
                .WithArg(val1)
                .WithArg(val2)
                .Build();

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStored(expected);

        }

    }
}