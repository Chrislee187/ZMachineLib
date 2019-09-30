using NUnit.Framework;
using ZMachineLib.Operations.OP0;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    public class PiracyTests : OperationsTestsBase<Piracy>
    {

        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_do_nothing()
        {
            Operation.Execute(null);

            Mockery.JumpedWith(true);
        }
    }
}