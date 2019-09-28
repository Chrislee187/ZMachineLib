using NUnit.Framework;
using ZMachineLib.Operations.OP2;

// ReSharper disable PossibleInvalidOperationException
namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:2 2 jl a b ?(label)
    /// <![CDATA[
    /// Jump if a < b (using a signed 16-bit comparison).
    /// ]]>
    /// </summary>

    public class JlTests : OperationsTestsBase<Jl>
    {


        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [TestCase((ushort)1, (ushort)2)]
        [TestCase((ushort)5, (ushort)7)]
        public void Should_jump_when_first_arg_less_than_second_arg(ushort firstArg, ushort secondArg)
        {
            var args = new OperandBuilder()
                .WithArg(firstArg)
                .WithArg(secondArg)
                .Build();

            Operation.Execute(args);

            Mockery.JumpedWith(true);
        }

        [TestCase((ushort)1, (ushort)1)]
        [TestCase((ushort)5, (ushort)4)]
        [TestCase((ushort)1, (ushort)0)]
        public void Should_NOT_jump_when_first_arg_greaterthan_or_equal_second_arg(ushort firstArg, ushort secondArg)
        {
            var args = new OperandBuilder()
                .WithArg(firstArg)
                .WithArg(secondArg)
                .Build();

            Operation.Execute(args);

            Mockery.JumpedWith(false);

        }
    }
}