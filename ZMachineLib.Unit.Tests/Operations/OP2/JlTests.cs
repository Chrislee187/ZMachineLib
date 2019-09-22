using NUnit.Framework;
using Shouldly;
using ZMachineLib.Operations.OP2;

// ReSharper disable PossibleInvalidOperationException
namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class JlTests : OperationsTestsBase
    {
        private Jl _op;
        private bool? _jumped;

        [SetUp]
        public void Setup()
        {
            var zMachine2 = new ZMachine2(null, null);
            _jumped = null;
            _op = new Jl(zMachine2, MemoryMock);
            _op.Jump = b => _jumped = b;

        }

        [TestCase((ushort)1, (ushort)2)]
        [TestCase((ushort)5, (ushort)7)]
        public void Should_jump_when_first_arg_less_than_second_arg(ushort firstArg, ushort secondArg)
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
        [TestCase((ushort)5, (ushort)4)]
        [TestCase((ushort)1, (ushort)0)]
        public void Should_NOT_jump_when_first_arg_greaterthan_or_equal_second_arg(ushort firstArg, ushort secondArg)
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