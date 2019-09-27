using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:15 F loadw array word-index -> (result)
    /// Stores array-->word-index(i.e., the word at address
    /// array+2*word-index, which must lie in static or dynamic
    /// memory).
    /// </summary>
    public class LoadWTests : OperationsTestsBase<LoadW>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((ushort)0x01, (ushort)0x02)]
        public void Should_store_the_value_from_addr_plus_index(ushort addr, ushort index)
        {
            var expectedValue = (ushort)0;
            var args = new OpArgBuilder()
                .WithValue(addr)
                .WithValue(expectedValue)
                .Build();

            Operation.Execute(args);


            VariableManagerMockery.UShortWasStored(expectedValue);

        }

    }
}