using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:139 B ret value
    /// Returns from the current routine with the value given.
    /// </summary>
    public class RemoveObjTestsTests : OperationsTestsBase<RemoveObj>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void TODO()
        {
            Assert.Inconclusive("To be tested");
            // Need to setup a few objects in a tree to properly tests

//            Mockery
//                .StartingPC(100, true);
//
//            var args = new OperandBuilder()
//                .WithArg(AnyValue)
//                .Build();
//
//            Operation.Execute(args);
//
//            Mockery
//                .ResultDestinationRetrievedFromPC()
//                .ResultStored(AnyValue);
        }

    }
}