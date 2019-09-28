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
        private const bool NoStoreExpected = false;

        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_set_call_routine_and_ignore_result()
        {
            const ushort packedRoutineAddress = 1234;
            const ushort routineArg1 = 1;
            const ushort routineArg2 = 2;
            const ushort routineArg3 = 3;
            var routineArgs = new[] {routineArg1, routineArg2, routineArg3};

            var args = new OperandBuilder()
                .WithArg(packedRoutineAddress)
                .WithArgs(routineArgs)
                .Build();

            Mockery
                .SetRoutineArgCount((byte) routineArgs.Length);

            Operation.Execute(args);

            var routineArgsOffset = routineArgs.Length * 2;// <= V4 Specific
            var unpackedAddress = ZMemory.GetPackedAddress(packedRoutineAddress);
            var expectedPC = (ushort) (unpackedAddress + routineArgsOffset);

            Mockery
                .StackFramePushed(NoStoreExpected, expectedPC)
                .RoutineArgsInitialisedFromMemory(routineArgs.Length)
                .RoutineArgsStoredOnStackFrame(routineArgs)
                .NoResultWasStored();
        }

        [Test]
        public void Should_do_nothing_if_no_routine_address_supplied()
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