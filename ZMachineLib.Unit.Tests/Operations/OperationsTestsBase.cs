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

        protected new void Setup()
        {
            base.Setup();

            Operation = (IOperation)Activator.CreateInstance(typeof(T), Mockery.Memory);

            MockPeekNextByte();
            MockJump(b => Jumped = b);


            // Default the destination for stored operation results tests to globals to avoid needing a stack
            SetNextDestinationAsGlobals();
        }
    }

    public abstract class OperationsTestsBase
    {
        protected IOperation Operation;
        const byte DestinationGlobals = 0x11;

        protected OperationsMockery Mockery;
        protected VariableManagerMockery VariableManagerMockery;
        protected ObjectTreeMockery ObjectTreeMockery;
        protected List<ushort> AnyArgs = new OperandBuilder().Build();
        protected const ushort AnyValue = 1;

        protected void Setup()
        {
            VariableManagerMockery = new VariableManagerMockery();
            ObjectTreeMockery = new ObjectTreeMockery();
            Mockery = new OperationsMockery(VariableManagerMockery, ObjectTreeMockery);
        }

        protected bool Jumped;
        protected void MockJump(Action<bool> operationJump) 
            => Operation.Jump = operationJump;
        protected void JumpedWith(bool value)
            => Jumped.ShouldBe(value);

        protected void MockPeekNextByte(byte value = 0)
            => Mockery.SetNextByte(value);

        protected void SetNextDestinationAsGlobals() 
            => MockPeekNextByte(DestinationGlobals);


    }
}