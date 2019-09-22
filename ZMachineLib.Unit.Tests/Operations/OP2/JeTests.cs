using NUnit.Framework;
using Shouldly;
using ZMachineLib.Operations.OP2;

// ReSharper disable PossibleInvalidOperationException

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class JeTests : OperationsTestsBase
    {
        private Je _op;
        private bool? _jumped;

        [SetUp]
        public void Setup()
        {
            var zMachine2 = new ZMachine2(null, null);
            _jumped = null;
            _op = new Je(MemoryMock);
            _op.Jump = b => _jumped = b;
            
        }

        [TestCase((ushort) 0, (ushort)1)]
        [TestCase((ushort)0, (ushort)2)]
        [TestCase((ushort)0, new ushort[] {1,2,3,4} )]
        public void Should_NOT_jump_when_no_values_matches_first_argument(ushort firstValue, params ushort[] toMatch)
        {
            var args = new OpArgBuilder()
                .WithValue(firstValue)
                .WithValues(toMatch)
                .Build();

            _op.Execute(args);

            _jumped.HasValue.ShouldBeTrue();
            _jumped.Value.ShouldBeFalse();
        }

        [TestCase((ushort)1, (ushort)1)]
        [TestCase((ushort)2, (ushort)2)]
        [TestCase((ushort)3, new ushort[] { 1, 2, 3, 4 })]
        public void Should_jump_when_a_value_matches_first_argument(ushort firstValue, params ushort[] toMatch)
        {
            var args = new OpArgBuilder()
                .WithValue(firstValue)
                .WithValues(toMatch)
                .Build();

            _op.Execute(args);

            _jumped.HasValue.ShouldBeTrue();
            _jumped.Value.ShouldBeTrue();
        }
    }
}