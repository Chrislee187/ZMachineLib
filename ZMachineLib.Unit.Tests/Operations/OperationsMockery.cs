using System;
using Moq;
using Shouldly;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class OperationsMockery
    {
        /// <summary>
        /// NOTE: Do we want subclasses per Mock<> instance?
        /// Pros
        ///  * Small classes
        ///
        /// Cons
        ///  * ?
        /// </summary>
        private readonly Mock<IZMemory> _memoryMock;
        private readonly Mock<IZObjectTree> _objectsMock;
        private readonly Mock<IVariableManager> _variablesMock;
        private readonly ZStack _zStack;
        private readonly Mock<IMemoryManager> _memoryManager;

        public IZMemory Memory => _memoryMock.Object;

        public OperationsMockery()
        {
            _memoryMock = new Mock<IZMemory>();
            _objectsMock = new Mock<IZObjectTree>();
            _variablesMock = new Mock<IVariableManager>();
            _memoryManager = new Mock<IMemoryManager>();
            _memoryMock
                .SetupGet(m => m.VariableManager)
                .Returns(_variablesMock.Object);

            _memoryMock
                .SetupGet(m => m.Manager)
                .Returns(_memoryManager.Object);

            _memoryMock
                .SetupGet(m => m.ObjectTree)
                .Returns(_objectsMock.Object);

            _zStack = new ZStack();
            _memoryMock
                .SetupGet(m => m.Stack)
                .Returns(_zStack);
        }

        /// <summary>
        /// Setup up the next call to get a variable from memory to return <paramref name="value"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperationsMockery VariableRetrieves(ushort value)
        {
            _variablesMock.Setup(m
                    => m.GetUShort(
                        It.IsAny<byte>(),
                        It.IsAny<bool>())
                )
                .Returns(value);
            return this;
        }

        /// <summary>
        /// Verifies a word (short/ushort) result was stored
        /// </summary>
        public OperationsMockery ResultStored(ushort expected)
        {
            _variablesMock.Verify(m => m.Store(
                It.IsAny<byte>(),
                It.Is<ushort>(v => v == expected),
                It.Is<bool>(b => b)), Times.Once);
            return this;
        }
        /// <summary>
        /// Verifies a word (short/ushort) result was stored
        /// </summary>
        public OperationsMockery ResultStored(short expected)
            => ResultStored((ushort)expected);

        /// <summary>
        /// Verifies a byte result was stored
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public OperationsMockery ResultStoredWasByte(byte expected)
        {
            _variablesMock.Verify(m => m.Store(
                It.IsAny<byte>(),
                It.Is<byte>(v => v == expected)), Times.Once);

            return this;
        }

        /// <summary>
        /// Verifies that the StoreResult for the current stackframe is false
        /// </summary>
        public OperationsMockery ResultWillBeStored()
        {
            _zStack.Peek().StoreResult.ShouldBeTrue();

            return this;
        }

        /// <summary>
        /// Verifies that no call was made to set a destination variable and
        /// that the StoreResult for the current stackframe is false
        /// </summary>
        
        public OperationsMockery NoResultWasStored()
        {
            if (_zStack.Count > 0)
            {
                _zStack.Peek().StoreResult.ShouldBeFalse();
            }
            _variablesMock.Verify(m => m.Store(
                    It.IsAny<byte>(),
                    It.IsAny<byte>())
                , Times.Never);

            _variablesMock.Verify(m => m.Store(
                    It.IsAny<byte>(),
                    It.IsAny<ushort>(),
                    It.IsAny<bool>())
                , Times.Never);

            return this;
        }
        /// <summary>
        /// Verifies that a destination variable type was read and implicitly that
        /// the program counter is incremented by one
        /// </summary>
        public OperationsMockery ResultDestinationRetrievedFromPC()
        {
            // TODO: ZMemory unit tests.
            // NOTE: We would like to check that the PC was used
            // and incremented here (as 'Store' operations use a byte
            // from the PC to identify the destination for the result,
            // and as such the PC is incremented by one.
            //
            // The method we mock here shows why, for testing purposes, you
            // typically only want a method to do ONE thing. As we cannot explicitly
            // test both the 'Get' and the 'Inc' here, we will have to rely on
            // ZMemory unit tests to ensure both these happen correctly.
            _memoryMock.Verify(
                m => m.GetCurrentByteAndInc(),
                Times.Once);
            return this;
        }


        /// <summary>
        /// Verifies the <paramref name="argCount"/> number of calls were made to retrieve
        /// values for local var initialisation
        /// </summary>
        /// <param name="argCount"></param>
        /// <returns></returns>
        public OperationsMockery LocalVariablesInitialisedFromMemory(int argCount)
        {
            // V4 Specific
            _memoryManager
                .Verify(m 
                    => m.GetUShort(It.IsAny<int>()), 
                    Times.AtLeast(argCount));

            return this;
        }

        /// <summary>
        /// Verifies that the values from <paramref name="routineArgs"/> are stored in the
        ///  routines local variables
        /// </summary>
        /// <param name="routineArgs"></param>
        /// <returns></returns>
        public OperationsMockery RoutineArgsStoredInLocalVariables(ushort[] routineArgs)
        {
            var zStackFrame = _zStack.Peek();

            for (int i = 0; i < routineArgs.Length; i++)
            {
                zStackFrame.Variables[i].ShouldBe(routineArgs[i]);
            }

            return this;
        }

        /// <summary>
        /// Sets the next byte returned, for use by Call() when handling routine arguments
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperationsMockery SetRoutineArgCount(byte value) => SetNextByte(value);

        public OperationsMockery AssertStackIsEmpty()
        {
            Should.Throw<Exception>(() => _zStack.Peek());
            return this;
        }


        public OperationsMockery SetNextByte(byte value)
        {
            _memoryMock
                .Setup(m => m.GetCurrentByteAndInc())
                .Returns(value);
            return this;
        }
        public OperationsMockery SetNextObject(IZMachineObject value)
        {
            _objectsMock
                .Setup(m => m.GetOrDefault(It.IsAny<ushort>()))
                .Returns(value);
            return this;
        }

        public OperationsMockery JumpedWith(bool value)
        {
            // TODO: Testing of the jump/branch needs to be a little better i think
            _memoryMock.Verify(m => m.Jump(It.Is<bool>(b => b.Equals(value))));
            return this;
        }

        public OperationsMockery ProgramCounterEquals(in ushort expectedPC)
        {
            var zStackFrame = _zStack.Peek();
            zStackFrame.PC.ShouldBe(expectedPC);

            return this;
        }
    }
}