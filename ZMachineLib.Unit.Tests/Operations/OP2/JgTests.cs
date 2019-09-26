using NUnit.Framework;
using Shouldly;
using ZMachineLib.Operations.OP2;

// ReSharper disable PossibleInvalidOperationException

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class JgTests : OperationsTestsBase
    {
        private Jg _op;
        private bool? _jumped;

        [SetUp]
        public void SetUp()
        {
            _jumped = null;
            _op = new Jg(MemoryMock);
            _op.Jump = b => _jumped = b;

        }

        [TestCase((ushort)2, (ushort)1)]
        [TestCase((ushort)7, (ushort)5)]
        public void Should_jump_when_first_arg_greaterthan_second_arg(ushort firstArg, ushort secondArg)
        {
            var args = new OpArgBuilder()
                .WithValue(firstArg)
                .WithValue(secondArg)
                .Build();

            _op.Execute(args);

            _jumped.HasValue.ShouldBeTrue();
            _jumped.Value.ShouldBeTrue();
        }

        [TestCase((ushort)1, (ushort)1)]
        [TestCase((ushort)4, (ushort)5)]
        [TestCase((ushort)0, (ushort)1)]
        public void Should_NOT_jump_when_first_arg_lessthan_or_equal_second_arg(ushort firstArg, ushort secondArg)
        {
            var args = new OpArgBuilder()
                .WithValue(firstArg)
                .WithValue(secondArg)
                .Build();

            _op.Execute(args);

            _jumped.HasValue.ShouldBeTrue();
            _jumped.Value.ShouldBeFalse();
        }
    }
}