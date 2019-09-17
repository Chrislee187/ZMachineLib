using Moq;

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
        

        public VariableManagerMockery SetupGetWord(ushort returnValue)
        {
            _variableManagerMock.Setup(m
                    => m.GetWord(
                        It.IsAny<byte>(),
                        It.IsAny<bool>())
                )
                .Returns(returnValue);

            return this;
        }

        public VariableManagerMockery VerifyStoreWord(ushort expectedValue)
        {
            _variableManagerMock.Verify(m => m.StoreWord(
                It.IsAny<byte>(),
                It.Is<ushort>(v => v == expectedValue),
                It.Is<bool>(b => b)), Times.Once);

            return this;
        }
    }
}