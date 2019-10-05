using NUnit.Framework;
using ZMachineLib.Operations.OP0;

namespace ZMachineLib.Unit.Tests.Operations.OP0
{
    /// <summary>
    /// 0OP:187 B new_line
    /// Print carriage return.
    /// </summary>
    public class NopTests : OperationsTestsBase<Nop>
    {

        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_do_nothing()
        {
            Mockery.StrictMode();
            InitOperations();

            Operation.Execute(null);
        }
    }
}