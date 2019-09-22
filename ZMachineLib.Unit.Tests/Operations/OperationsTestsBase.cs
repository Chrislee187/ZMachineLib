using System.Collections.Generic;
using Castle.DynamicProxy.Generators.Emitters;
using Moq;
using Shouldly;
using ZMachineLib.Content;
using ZMachineLib.Managers;
using ZMachineLib.Operations;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class OperationsTestsBase
    {
        protected VariableManagerMockery VariableManagerMockery;
        protected ObjectManagerMockery ObjectManagerMockery;
        protected ObjectTreeMockery ObjectTreeMockery;
        protected ZMachine2 ZMachine2;
        private bool? _jumped;
        protected List<ushort> AnyArgs = new OpArgBuilder().Build();
        protected const ushort AnyValue = 1;
        protected IZMemory MemoryMock;

        protected void Setup()
        {
            ZMachine2 = new ZMachine2(null, null);
            VariableManagerMockery = new VariableManagerMockery();
            ObjectManagerMockery = new ObjectManagerMockery();
            ObjectTreeMockery = new ObjectTreeMockery();

            var memoryMock = new Mock<IZMemory>();
            memoryMock
                .SetupGet(m => m.VariableManager)
                .Returns(VariableManagerMockery.Object);

            memoryMock
                .SetupGet(m => m.Manager)
                .Returns(new Mock<IMemoryManager>().Object);

            memoryMock
                .SetupGet(m => m.ObjectTree)
                .Returns(ObjectTreeMockery.Object);
            MemoryMock = memoryMock.Object;

        }
        protected void MockJump(IOperation op)
        {
            op.Jump = b => _jumped = b;
        }
        protected void MockPeekNextByte(IOperation op)
        {
            op.GetNextByte = () => 0;
        }
        protected void JumpedWith(bool value)
        {
            _jumped.HasValue.ShouldBeTrue();
            // ReSharper disable once PossibleInvalidOperationException
            _jumped.Value.ShouldBe(value);
        }

    }
}