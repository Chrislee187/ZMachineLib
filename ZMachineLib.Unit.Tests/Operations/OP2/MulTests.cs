using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:23 17 div a b -> (result)
    /// Signed 16-bit multiplication.
    /// </summary>
    public class MulTests : OperationsTestsBase<Mul>
    {

        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((short)1, (short)2, (short)2)]
        [TestCase((short)2, (short)2, (short)4)]
        [TestCase((short)2, (short)-2,(short)-4)]
        public void Should_store_AND_result(short val1, short val2, short expected)
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