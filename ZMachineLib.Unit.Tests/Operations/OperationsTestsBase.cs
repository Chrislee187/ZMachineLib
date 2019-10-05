using System;
using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Operations;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class OperationsTestsBase<T> : OperationsTestsBase where T : IOperation
    {
        protected const byte AnyVariable = 0x7f;

        protected new void Setup()
        {
            base.Setup();

            InitOperations();
        }

        protected void InitOperations()
        {
            if (OperationUsesUserIo())
            {
                var args = new object[] {Mockery.Memory, Mockery.UserIo};
                Operation = (IOperation) Activator.CreateInstance(typeof(T), args: args);
            }
            else
            {
                Operation = (IOperation) Activator.CreateInstance(typeof(T), Mockery.Memory);
            }

            MockPeekNextByte();

            // Default the destination for stored operation results tests to globals to avoid needing a stack
            SetNextDestinationAsGlobals();
        }

        private bool OperationUsesUserIo() 
            => typeof(T).GetConstructor(new []{typeof(IZMemory), typeof(IUserIo)}) != null;
    }

    public abstract class OperationsTestsBase
    {
        protected IOperation Operation;
        const byte DestinationGlobals = 0x11;

        protected OperationsMockery Mockery;


        protected List<ushort> AnyArgs = new OperandBuilder().Build();
        protected const ushort AnyValue = 1;

        protected void Setup()
        {
            Mockery = new OperationsMockery();
        }

        protected void MockPeekNextByte(byte value = 0)
            => Mockery.SetCurrentByte(value);

        protected void SetNextDestinationAsGlobals() 
            => MockPeekNextByte(DestinationGlobals);


    }
}