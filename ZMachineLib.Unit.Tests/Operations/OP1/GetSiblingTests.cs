using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:129 1 get_sibling object -> (result) ?(label)
    /// Get next object in tree, branching if this exists, i.e. is not 0.
    /// </summary>
    public class GetSiblingTests : OperationsTestsBase<GetSibling>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_store_sibling_and_jump()
        {
            const int objectNumber = 10;
            const int sibling = 5;

            var args = new OperandBuilder()
                .WithArg(objectNumber)
                .Build();


            var zObj = new ZMachineObjectBuilder()
                .WithObjectNumber(objectNumber)
                .WithSibling(sibling)
                .Build();

            Mockery
                .SetNextObject(zObj);

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStoredWasByte(sibling)
                .JumpedWith(true)
                ;
        }

        [Test]
        public void Should_not_jump_when_sibling_is_zero()
        {
            const int objectNumber = 10;
            const int sibling = 0;

            var args = new OperandBuilder()
                .WithArg(objectNumber)
                .Build();


            var zObj = new ZMachineObjectBuilder()
                .WithObjectNumber(objectNumber)
                .WithSibling(sibling)
                .Build();

            Mockery
                .SetNextObject(zObj);

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStoredWasByte(sibling)
                .JumpedWith(false)
                ;
        }
    }
}