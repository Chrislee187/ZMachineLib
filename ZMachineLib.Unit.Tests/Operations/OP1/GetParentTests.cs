using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:131 3 get_parent object -> (result)
    /// Get parent object
    /// (note that this has NO "branch if exists" clause as Get Child has).
    /// </summary>
    public class GetParentTests : OperationsTestsBase<GetParent>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_store_parent_and_never_jump()
        {
            const int objectNumber = 10;
            const int parent = 25;

            var args = new OperandBuilder()
                .WithArg(AnyVariable)
                .Build();


            var zObj = new ZMachineObjectBuilder()
                .WithObjectNumber(objectNumber)
                .WithParent(parent)
                .Build();

            Mockery
                .SetNextObject(zObj);

            Operation.Execute(args);

            Mockery
                .ResultDestinationRetrievedFromPC()
                .ResultStoredWasByte(parent)
                .NeverJumps()
                ;
        }
    }
}