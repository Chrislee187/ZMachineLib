using Moq;
using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:135 7 print_addr byte-address-of-string
    /// Print(Z-encoded) string at given byte address, in dynamic or static memory.
    /// </summary>
    public class PrintAddrTests : OperationsTestsBase<PrintAddr>
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
                .Printed("some text")
                ;
        }
    }
}