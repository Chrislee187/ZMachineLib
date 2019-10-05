using System;
using NUnit.Framework;
using ZMachineLib.Operations.OP0;

namespace ZMachineLib.Unit.Tests.Operations.OP0
{
    /// <summary>
    /// 0OP:187 B new_line
    /// Print carriage return.
    /// </summary>
    public class NewLineTests : OperationsTestsBase<Newline>
    {

        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_output_a_single_newline()
        {
            Operation.Execute(null);

            Mockery
                .Printed(Environment.NewLine)
                ;
        }
    }
}