using NUnit.Framework;
using ZMachineLib.Operations.OP2;


namespace ZMachineLib.Unit.Tests.Operations
{
    public class DecCheckTests
    {
        private DecCheck _op;

        [SetUp]
        public void Setup()
        {
            var zMachine2 = new ZMachine2(null, null);
            _op = new DecCheck(zMachine2);
        }

        [NUnit.Framework.Test]
        public void DecCheck()
        {
            var args = new OpArgBuilder()

                .Build();

            _op.Execute(args);

            Assert.Fail();
        }

    }
}