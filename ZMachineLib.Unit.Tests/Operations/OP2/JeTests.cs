using NUnit.Framework;
using ZMachineLib.Operations.OP2;

// ReSharper disable PossibleInvalidOperationException

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:1 1 je a b c d ?(label)
    /// Jump if a is equal to any of the subsequent operands.
    /// (Thus @je a never jumps and @je a b jumps if a = b.)
    /// je with just 1 operand is not permitted.
    /// </summary>
    public class JeTests : OperationsTestsBase<Je>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((ushort) 0, new ushort[] { 1})]
        [TestCase((ushort)0, new ushort[] { 2 })]
        [TestCase((ushort)0, new ushort[] {1,2,3,4} )]
        public void Should_NOT_jump_when_no_values_matches_first_argument(ushort firstValue, params ushort[] toMatch)
        {
            var args = new OperandBuilder()
                .WithArg(firstValue)
                .WithArgs(toMatch)
                .Build();

            Operation.Execute(args);

            JumpedWith(false);
        }

        [TestCase((ushort)1, (ushort)1)]
        [TestCase((ushort)2, (ushort)2)]
        [TestCase((ushort)3, new ushort[] { 1, 2, 3, 4 })]
        public void Should_jump_when_a_value_matches_first_argument(ushort firstValue, params ushort[] toMatch)
        {
            var args = new OperandBuilder()
                .WithArg(firstValue)
                .WithArgs(toMatch)
                .Build();

            Operation.Execute(args);

            JumpedWith(true);
        }
    }
}