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


        [TestCase((ushort)0b0, (ushort)0b0, (ushort)0b0)]
        [TestCase((ushort)0b0, (ushort)0b1, (ushort)0b0)]
        [TestCase((ushort)0b1, (ushort)0b0, (ushort)0b0)]
        [TestCase((ushort)0b1, (ushort)0b1, (ushort)0b1)]
        public void Should_store_bitwise_AND_result(ushort argA, ushort argB, ushort expected)
        {
            var args = new OperandBuilder()
                .WithArg(argA)
                .WithArg(argB)
                .Build();

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStored(expected);

        }

    }
}