using NUnit.Framework;
using ZMachineLib.Operations.OP2;
using TestAttribute = NUnit.Framework.TestAttribute;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class TestTests : OperationsTestsBase
    {
        private Test _op;


        [SetUp]
        public void SetUp()
        {
            Setup();
            _op = new Test(MemoryMock);
            MockJump(_op);
        }

        [Test]
        public void Should_jump_if_all_flags_set()
        {
            ushort bitmap = 0xFF;
            ushort flags = 0x0F;

            var args = new OpArgBuilder()
                .WithValue(bitmap)
                .WithValue(flags)
                .Build();

            _op.Execute(args);

            JumpedWith(true);
        }
        [Test]
        public void Should_NOT_jump_if_any_flags_NOT_set()
        {
            ushort bitmap = 0x0E;
            ushort flags = 0x0F;

            var args = new OpArgBuilder()
                .WithValue(bitmap)
                .WithValue(flags)
                .Build();

            _op.Execute(args);

            JumpedWith(false);
        }

    }
}