using NUnit.Framework;
using ZMachineLib.Content;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:143 F 5 call_1n routine
    /// Executes routine() and throws away result.
    /// </summary>
    public class Call1NTests : OperationsTestsBase<Call1N>
    {

        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_set_call_routine_and_ignore_result()
        {
            const ushort packedRoutineAddress = 1234;
            var routineArgs = new[] {(ushort) 1, (ushort) 2, (ushort) 3};

            var args = new OperandBuilder()
                .WithArg(packedRoutineAddress)
                .WithArgs(routineArgs)
                .Build();

            Mockery
                .SetRoutineArgCount((byte) routineArgs.Length);

            Operation.Execute(args);

            var routineArgsOffset = routineArgs.Length * 2;// <= V4 Specific
            var unpackedAddress = ZMemory.UnpackedAddress(packedRoutineAddress);
            var expectedPC = (ushort) (unpackedAddress + routineArgsOffset);

            Mockery
                .ProgramCounterEquals(expectedPC)
                .NoResultWasStored()
                .LocalVariablesInitialisedFromMemory(routineArgs.Length)
                .RoutineArgsStoredInLocalVariables(routineArgs)
                ;
        }

        [Test]
        public void When_no_routine_address_supplied_set_result_to_0()
        {
            const ushort packedRoutineAddress = 0;
            var args = new OperandBuilder()
                .WithArg(packedRoutineAddress)
                .Build();

            Mockery
                .AssertStackIsEmpty();

            Mockery
                .SetRoutineArgCount(0); 

            Operation.Execute(args);

            Mockery
                .AssertStackIsEmpty()
                .NoResultWasStored();
        }
    }
}