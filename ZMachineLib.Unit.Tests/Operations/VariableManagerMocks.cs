using Moq;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class VariableManagerMocks
    {
        private Mock<IVariableManager> _variableManagerMock;

        public VariableManagerMocks()
        {
            _variableManagerMock = new Mock<IVariableManager>();
        }

        public IVariableManager Object => _variableManagerMock.Object;
        

        public VariableManagerMocks SetupGetWord(ushort returnValue)
        {
            _variableManagerMock.Setup(m
                    => m.GetWord(
                        It.IsAny<byte>(),
                        It.IsAny<bool>())
                )
                .Returns(returnValue);

            return this;
        }

        public VariableManagerMocks VerifyStoreWord(ushort expectedValue)
        {
            _variableManagerMock.Verify(m => m.StoreWord(
                It.IsAny<byte>(),
                It.Is<ushort>(v => v == expectedValue),
                It.Is<bool>(b => b)), Times.Once);

            return this;
        }
    }

    public class ObjectManagerMocks
    {
        private Mock<IObjectManager> _objectManagerMock;

        public ObjectManagerMocks()
        {
            _objectManagerMock = new Mock<IObjectManager>();
            _objectManagerMock
                .Setup(o => o.GetObjectName(It.IsAny<ushort>()))
                .Returns("");
        }

        public IObjectManager Object => _objectManagerMock.Object;


        public ObjectManagerMocks SetupGetObjectAddress(ushort returnValue)
        {
            _objectManagerMock.Setup(m
                    => m.GetObjectAddress(It.IsAny<ushort>())
                )
                .Returns(returnValue);

            return this;
        }

        public ObjectManagerMocks SetupGetObjectNumber(ushort returnValue)
        {
            _objectManagerMock.Setup(m
                    => m.GetObjectNumber(It.IsAny<ushort>())
                )
                .Returns(returnValue);

            return this;
        }
        public ObjectManagerMocks SetupGetObjectParent(ushort returnValue)
        {
            _objectManagerMock.Setup(m
                    => m.GetObjectParent(It.IsAny<ushort>())
                )
                .Returns(returnValue);

            return this;
        }
    }
}