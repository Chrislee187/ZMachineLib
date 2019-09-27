using Moq;
using ZMachineLib.Managers;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class VariableManagerMockery
    {
        private readonly Mock<IVariableManager> _variableManagerMock;

        public VariableManagerMockery()
        {
            _variableManagerMock = new Mock<IVariableManager>();
        }

        public IVariableManager Object => _variableManagerMock.Object;
        

        public VariableManagerMockery UShortWasRetrieved(ushort returnValue)
        {
            _variableManagerMock.Setup(m
                    => m.GetUShort(
                        It.IsAny<byte>(),
                        It.IsAny<bool>())
                )
                .Returns(returnValue);

            return this;
        }

        public VariableManagerMockery UShortWasStored(ushort expectedValue)
        {
            _variableManagerMock.Verify(m => m.StoreUShort(
                It.IsAny<byte>(),
                It.Is<ushort>(v => v == expectedValue),
                It.Is<bool>(b => b)), Times.Once);

            return this;
        }
        public VariableManagerMockery ByteWasStored(byte expectedValue)
        {
            _variableManagerMock.Verify(m => m.StoreByte(
                It.IsAny<byte>(),
                It.Is<byte>(v => v == expectedValue)
                ), Times.Once);

            return this;
        }
    }
}