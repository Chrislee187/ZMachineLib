using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:130 2 get_child object -> (result) ?(label)
    /// Get first object contained in given object,
    /// branching if this exists,
    /// i.e. is not nothing (i.e., is not 0).
    /// </summary>
    public class GetChildTests : OperationsTestsBase<GetChild>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_store_child_and_jump()
        {
            const int objectNumber = 10;
            const int child = 15;

            var args = new OperandBuilder()
                .WithArg(AnyVariable)
                .Build();


            var zObj = new ZMachineObjectBuilder()
                .WithObjectNumber(objectNumber)
                .WithChild(child)
                .Build();

            Mockery
                .SetNextObject(zObj);

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStoredWasByte(child)
                .JumpedWith(true)
                ;
        }

        [Test]
        public void Should_not_jump_when_child_is_zero()
        {
            const int objectNumber = 10;
            const int child = 0;

            var args = new OperandBuilder()
                .WithArg(objectNumber)
                .Build();


            var zObj = new ZMachineObjectBuilder()
                .WithObjectNumber(objectNumber)
                .WithSibling(child)
                .Build();

            Mockery
                .SetNextObject(zObj);

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStoredWasByte(child)
                .JumpedWith(false)
                ;
        }
    }
}