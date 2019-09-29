using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:141 D print_paddr packed-address-of-string
    /// Print the(Z-encoded) string at the given packed address in high memory.
    /// </summary>
    public class PrintPAddrTests : OperationsTestsBase<PrintPAddr>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_print_the_text()
        {
            var args = new OperandBuilder()
                .WithArg(0)
                .Build();

            Mockery
                .ZsciiStringReturns("some text");

            Operation.Execute(args);




            Mockery
                // TODO: Check that a packed address was used
                .Printed("some text")
                ;
        }
    }
}