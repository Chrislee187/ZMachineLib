using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// jump
    /// 1OP:140 C jump ? (label)
    ///
    /// Jump(unconditionally) to the given label.
    /// (This is not a branch instruction and the operand is a 2-byte
    /// signed offset to apply to the program counter.)
    /// It is legal for this to jump into a different routine
    /// (which should not change the routine call state),
    /// although it is considered bad practice to do so
    /// and the Txd disassembler is confused by it.
    ///
    /// The destination of the jump opcode is:
    ///
    /// Address after instruction + Offset - 2
    ///
    /// This is analogous to the calculation for branch offsets.
    /// </summary>
    public class JumpTests : OperationsTestsBase<Jump>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((short)10, (short) 108 )]
        [TestCase((short)0, (short) 98)]
        [TestCase((short)-110, (short) -12)]
        public void Should_increment_program_counter_by_offset_minus_two(short offset, short expected)
        {
            Mockery
                .StartingPC(100);

            var args = new OperandBuilder()
                .WithArg(offset)
                .Build();

            Operation.Execute(args);

            Mockery
                .ProgramCounterEquals(expected);
        }
    }
}