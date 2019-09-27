using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:20 14 add a b -> (result)
    /// Signed 16-bit addition.
    /// </summary>
    public class AddTests : OperationsTestsBase<Add>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((short) 1, (short) 2, 3)]
        [TestCase((short)-1, (short)-1, -2)]
        public void Should_store_Add_result(short val1, short val2, short result)
        {
            var args = new OpArgBuilder()
                .WithValue((ushort)val1)
                .WithValue((ushort)val2)
                .Build();

            Operation.Execute(args);

            VariableManagerMockery
                .UShortWasStored((ushort) result);
        }
    }
}