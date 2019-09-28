using NUnit.Framework;
using ZMachineLib.Operations.OP2;
using TestAttribute = NUnit.Framework.TestAttribute;

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


        [Test]
        public void Should_store_the_value_from_addr_plus_index()
        {
            // TODO: Crap test , doesn't check the address used to retrieve the array item is correct
            ushort addr = 100;
            ushort index = 200;
            var expectedValue = (ushort)0;
            var args = new OperandBuilder()
                .WithArg(addr)
                .WithArg(index)
                .Build();

            Operation.Execute(args);


            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStored(expectedValue);


        }

    }
}