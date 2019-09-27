using System;
using System.Collections.Generic;
using Moq;
using Shouldly;
using ZMachineLib.Content;
using ZMachineLib.Managers;
using ZMachineLib.Operations;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class OperationsTestsBase<T> : OperationsTestsBase where T : IOperation
    {
        // TODO: Make a MemoryMockery
        protected Mock<IZMemory> MemoryMock;

        protected new void Setup()
        {
            base.Setup();
            MemoryMock = new Mock<IZMemory>();
            Operation = (IOperation)Activator.CreateInstance(typeof(T), MemoryMock.Object);

            MockPeekNextByte();
            MockJump(b => Jumped = b);

            MemoryMock
                .SetupGet(m => m.VariableManager)
                .Returns(VariableManagerMockery.Object);

            MemoryMock
                .SetupGet(m => m.Manager)
                .Returns(new Mock<IMemoryManager>().Object);

            MemoryMock
                .SetupGet(m => m.ObjectTree)
                .Returns(ObjectTreeMockery.Object);

            // Default the destination for stored operation results tests to globals to avoid needing a stack
            SetNextDestinationGlobals();
        }
    }

    public abstract class OperationsTestsBase
    {
        protected IOperation Operation;
        const byte DestinationGlobals = 0x11;


        protected VariableManagerMockery VariableManagerMockery;
        protected ObjectTreeMockery ObjectTreeMockery;
        protected List<ushort> AnyArgs = new OpArgBuilder().Build();
        protected const ushort AnyValue = 1;

        protected void Setup()
        {
            VariableManagerMockery = new VariableManagerMockery();
            ObjectTreeMockery = new ObjectTreeMockery();
        }

        protected bool Jumped;
        protected void MockJump(Action<bool> operationJump) 
            => Operation.Jump = operationJump;
        protected void JumpedWith(bool value)
            => Jumped.ShouldBe(value);

        protected void MockPeekNextByte(byte value = 0) 
            => Operation.GetCurrentByteAndInc = () => value;

        protected void SetNextDestinationGlobals() 
            => MockPeekNextByte(DestinationGlobals);


    }
}