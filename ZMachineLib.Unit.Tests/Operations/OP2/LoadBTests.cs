using NUnit.Framework;
using ZMachineLib.Operations.OP2;
using TestAttribute = NUnit.Framework.TestAttribute;

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

        [Test]
        public void Should_store_the_value_from_addr_plus_index()
        {
            // TODO: Crap test , doesn't check the address used to retrieve the array item is correct
            ushort addr = 1234;
            ushort index = 12;
            var args = new OperandBuilder()
                .WithArg(addr)
                .WithArg(index)
                .Build();

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStoredWasByte(0);
        }
    }
}