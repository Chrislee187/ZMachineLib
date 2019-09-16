using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using ZMachineLib.Operations.OP2;


namespace ZMachineLib.Unit.Tests.Operations
{
    public class DecCheckTests
    {
        private DecCheck _op;
        private bool? _jumped;
        private Mock<Func<byte, bool, ushort>> _getVariableMock;

        [SetUp]
        public void Setup()
        {
            var zMachine2 = new ZMachine2(null, null);
            _jumped = null;
            _op = new DecCheck(zMachine2);
            _getVariableMock = new Mock<Func<byte, bool, ushort>>();

        }

        [NUnit.Framework.Test]
        public void DecCheck()
        {
            var args = new OpArgBuilder()

                .Build();

            _op.Execute(args);

            _jumped.HasValue.ShouldBeTrue();
            _jumped.Value.ShouldBeTrue();
        }

    }
}