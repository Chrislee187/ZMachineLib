using NUnit.Framework;
using ZMachineLib.Operations.OP2;

// ReSharper disable PossibleInvalidOperationException

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:3 3 jg a b ?(label)
    /// Jump if a > b(using a signed 16-bit comparison).
    /// </summary>

    public class JgTests : OperationsTestsBase<Jg>
    {

        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((ushort)2, (ushort)1)]
        [TestCase((ushort)7, (ushort)5)]
        public void Should_jump_when_first_arg_greater_than_second_arg(ushort firstArg, ushort secondArg)
        {
            var args = new OperandBuilder()
                .WithArg(firstArg)
                .WithArg(secondArg)
                .Build();

            Operation.Execute(args);

            Mockery.JumpedWith(true);
        }

        [TestCase((ushort)1, (ushort)1)]
        [TestCase((ushort)4, (ushort)5)]
        [TestCase((ushort)0, (ushort)1)]
        public void Should_NOT_jump_when_first_arg_less_than_or_equal_second_arg(ushort firstArg, ushort secondArg)
        {
            var args = new OperandBuilder()
                .WithArg(firstArg)
                .WithArg(secondArg)
                .Build();

            Operation.Execute(args);

            Mockery.JumpedWith(false);
        }
    }
}