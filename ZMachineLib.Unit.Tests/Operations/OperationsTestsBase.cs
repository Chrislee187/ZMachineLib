using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ZMachineLib.Content;
using ZMachineLib.Operations;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class OperationsTestsBase<T> : OperationsTestsBase where T : IOperation
    {

        public const byte AnyVariable = 0x7f;

        protected void Setup()
        {
            base.Setup();


            if (OperationUsesUserIo<T>())
            {
                var args = new object[]{Mockery.Memory, Mockery.UserIo};
                Operation = (IOperation)Activator.CreateInstance(typeof(T), args: args);
            }
            else
            {
                Operation = (IOperation)Activator.CreateInstance(typeof(T), Mockery.Memory);
            }

            MockPeekNextByte();

            // Default the destination for stored operation results tests to globals to avoid needing a stack
            SetNextDestinationAsGlobals();
        }

        private bool OperationUsesUserIo<T>() 
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
            => Mockery.SetNextByte(value);

        protected void SetNextDestinationAsGlobals() 
            => MockPeekNextByte(DestinationGlobals);


    }
}