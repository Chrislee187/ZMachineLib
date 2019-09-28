using NUnit.Framework;
using ZMachineLib.Operations.OP2;
using TestAttribute = NUnit.Framework.TestAttribute;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:7 7 test bitmap flags? (label)
    /// Jump if all of the flags in bitmap are set(i.e. if bitmap & flags == flags).
    /// </summary>

    public class TestTests : OperationsTestsBase<Test>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_jump_if_all_flags_set()
        {
            ushort bitmap = 0xFF;
            ushort flags = 0x0F;

            var args = new OperandBuilder()
                .WithArg(bitmap)
                .WithArg(flags)
                .Build();

            Operation.Execute(args);

            Mockery.JumpedWith(true);
        }
        [Test]
        public void Should_NOT_jump_if_any_flags_NOT_set()
        {
            ushort bitmap = 0x0E;
            ushort flags = 0x0F;

            var args = new OperandBuilder()
                .WithArg(bitmap)
                .WithArg(flags)
                .Build();

            Operation.Execute(args);

            Mockery.JumpedWith(false);
        }

    }
}