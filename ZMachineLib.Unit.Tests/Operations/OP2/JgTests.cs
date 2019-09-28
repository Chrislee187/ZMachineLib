using NUnit.Framework;
using Shouldly;
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
        private bool? _jumped;

        [SetUp]
        public void SetUp()
        {
            Setup();
            _jumped = null;
            Operation.Jump = b => _jumped = b;

        }

        [TestCase((ushort)2, (ushort)1)]
        [TestCase((ushort)7, (ushort)5)]
        public void Should_jump_when_first_arg_greaterthan_second_arg(ushort firstArg, ushort secondArg)
        {
            var args = new OperandBuilder()
                .WithArg(firstArg)
                .WithArg(secondArg)
                .Build();

            Operation.Execute(args);

            _jumped.HasValue.ShouldBeTrue();
            _jumped.Value.ShouldBeTrue();
        }

        [TestCase((ushort)1, (ushort)1)]
        [TestCase((ushort)4, (ushort)5)]
        [TestCase((ushort)0, (ushort)1)]
        public void Should_NOT_jump_when_first_arg_lessthan_or_equal_second_arg(ushort firstArg, ushort secondArg)
        {
            var args = new OperandBuilder()
                .WithArg(firstArg)
                .WithArg(secondArg)
                .Build();

            Operation.Execute(args);

            _jumped.HasValue.ShouldBeTrue();
            _jumped.Value.ShouldBeFalse();
        }
    }
}