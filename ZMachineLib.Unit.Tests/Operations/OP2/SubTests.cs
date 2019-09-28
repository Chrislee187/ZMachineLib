using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:21 15 sub a b -> (result)
    /// Signed 16-bit subtraction.
    /// </summary>
    public class SubTests : OperationsTestsBase<Sub>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((short)2, (short)1, (short) 1)]
        [TestCase((short)-1, (short)1, (short) -2)]
        public void Should_store_AND_result(short val1, short val2, short expected)
        {
            var args = new OperandBuilder()
                .WithArg((ushort)val1)
                .WithArg((ushort)val2)
                .Build();

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStored(expected);
        }
    }
}