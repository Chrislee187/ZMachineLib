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
        /// NOTE: Do we want subclasses per Mock instance?
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
        private readonly Mock<IUserIo> _userIoMock;

        public IZMemory Memory => _memoryMock.Object;
        public IUserIo UserIo => _userIoMock.Object;

        public OperationsMockery()
        {

            _userIoMock = new Mock<IUserIo>();
            _memoryMock = new Mock<IZMemory>();
            _objectsMock = new Mock<IZObjectTree>();
            _variablesMock = new Mock<IVariableManager>();

            // TODO: Consider using strict mocking on the MemoryManager (maybe variables as well)
            // to ensure no side effects, specifically on memory pointers etc, typically strict mocking
            // can make the tests brittle but that should not be the case here as the core implementations
            // of operations are unlikely to change
            _memoryManager = new Mock<IMemoryManager>(/*MockBehavior.Strict*/);

            _memoryManager
                .Setup(m => m.AsSpan(It.IsAny<short>()))
                .Returns(new byte[] {});

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
        /// Setup up the next call to get a variable from memory to return <paramref name="value"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperationsMockery VariableRetrieves(short value)
        {
            _variablesMock.Setup(m
                    => m.GetUShort(
                        It.IsAny<byte>(),
                        It.IsAny<bool>())
                )
                .Returns((ushort)value);
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
        public OperationsMockery NoResultDestinationRetrieved()
        {
            _memoryMock.Verify(
                m => m.GetCurrentByteAndInc(),
                Times.Never);
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


        public OperationsMockery StartingPC(int pc, bool storeResult = false)
        {
            _zStack.Push(new ZStackFrame {PC = (uint) pc, StoreResult = storeResult});
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

        public OperationsMockery NeverJumps()
        {
            // TODO: Testing of the jump/branch needs to be a little better i think
            _memoryMock.Verify(m => m.Jump(It.IsAny<bool>()), Times.Never);
            return this;
        }
        public OperationsMockery ProgramCounterEquals(in ushort expectedPC)
        {
            _zStack.Peek().PC.ShouldBe(expectedPC);

            return this;
        }
        public OperationsMockery ProgramCounterEquals(in short expectedPC)
        {
            var pc = (short)(_zStack.Peek().PC);
            pc.ShouldBe(expectedPC);

            return this;
        }

        public OperationsMockery ZsciiStringReturns(string someText)
        {
            _memoryMock
                .Setup(m => m.GetZscii(It.IsAny<ushort>()))
                .Returns(someText);

            _memoryMock
                .Setup(m => m.GetZscii(It.IsAny<byte[]>()))
                .Returns(someText);
            return this;
        }

        public OperationsMockery Printed(string someText)
        {
            _userIoMock
                .Verify(m => m.Print(It.Is<string>(s => s == someText)));
            return this;
        }
    }
}