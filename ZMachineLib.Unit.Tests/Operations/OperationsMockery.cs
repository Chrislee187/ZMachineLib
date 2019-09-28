using Moq;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class OperationsMockery
    {
        private readonly Mock<IZMemory> _memoryMock;
        private readonly Mock<IZObjectTree> _objectsMock;
        private readonly Mock<IVariableManager> _variablesMock;

        public IZMemory Memory => _memoryMock.Object;

        public OperationsMockery()
        {
            _memoryMock = new Mock<IZMemory>();
            _objectsMock = new Mock<IZObjectTree>();
            _variablesMock = new Mock<IVariableManager>();
            _memoryMock
                .SetupGet(m => m.VariableManager)
                .Returns(_variablesMock.Object);

            _memoryMock
                .SetupGet(m => m.Manager)
                .Returns(new Mock<IMemoryManager>().Object);

            _memoryMock
                .SetupGet(m => m.ObjectTree)
                .Returns(_objectsMock.Object);

        }
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

        public OperationsMockery ResultStored(ushort expected)
        {
            _variablesMock.Verify(m => m.Store(
                It.IsAny<byte>(),
                It.Is<ushort>(v => v == expected),
                It.Is<bool>(b => b)), Times.Once);
            return this;
        }
        public OperationsMockery ResultStored(short expected)
            => ResultStored((ushort)expected);

        public OperationsMockery ResultStoredWasByte(byte expected)
        {
            _variablesMock.Verify(m => m.Store(
                It.IsAny<byte>(),
                It.Is<byte>(v => v == expected)), Times.Once);

            return this;
        }

        
        public OperationsMockery ResultDestinationRetrievedFromPC()
        {
            // NOTE: We would like to check that the PC was used
            // and incremented here (as 'Store' operations use a byte
            // from the PC to identify the destination for the result,
            // and as such the PC is incremented by one.
            //
            // The method we mock here shows why, for testing purposes, you
            // typically only want a method to do ONE thing. As we cannot explicitly
            // test both the 'Get' and the 'Inc' here, we will have to rely on
            // ZMemory unit tests to ensure both these happen correctly.
            // TODO: ZMemory unit tests.
            _memoryMock.Verify(
                m => m.GetCurrentByteAndInc(),
                Times.Once);
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
    }
}