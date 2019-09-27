using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:23 17 div a b -> (result)
    /// Signed 16-bit division.
    /// Division by zero should halt the interpreter with a suitable error message.
    /// </summary>

    public class DivTests : OperationsTestsBase<Div>
    {

        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((short)1, (short)2)]
        [TestCase((short)-1, (short)-1)]
        public void Should_store_AND_result(short val1, short val2)
        {
            var args = new OpArgBuilder()
                .WithValue((ushort)val1)
                .WithValue((ushort)val2)
                .Build();

            Operation.Execute(args);

            var expected = (ushort)(val1 / val2);
            VariableManagerMockery
                .UShortWasStored(expected);
        }
    }
}