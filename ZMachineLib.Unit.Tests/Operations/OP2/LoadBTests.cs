using NUnit.Framework;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:16 10 loadb array byte-index -> (result)
    /// Stores array->byte-index(i.e., the byte at address array+byte-index,
    /// which must lie in static or dynamic memory).
    /// </summary>
    public class LoadBTests : OperationsTestsBase<LoadB>
    {


        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((ushort)0x01, (ushort)0x02)]
        public void Should_store_the_value_from_addr_plus_index(ushort addr, ushort index)
        {
            var args = new OpArgBuilder()
                .WithValue(addr)
                .WithValue(index)
                .Build();

            Operation.Execute(args);

            var expectedValue = (byte) 0;
            VariableManagerMockery.ByteWasStored(expectedValue);

        }

    }
}