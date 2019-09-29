using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:128 0 jz a ?(label)
    /// Jump if a = 0.
    /// </summary>
    public class JzTests : OperationsTestsBase<Jz>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_jump_when_arg_is_zero()
        {
            var args = new OperandBuilder()
                .WithArg(0)
                .Build();

            Operation.Execute(args);

            Mockery
                .JumpedWith(true)
                ;
        }

        [Test]
        public void Should_not_jump_when_arg_is_non_zero()
        {
            var args = new OperandBuilder()
                .WithArg(1)
                .Build();

            Operation.Execute(args);

            Mockery
                .JumpedWith(false)
                ;
        }
    }
}