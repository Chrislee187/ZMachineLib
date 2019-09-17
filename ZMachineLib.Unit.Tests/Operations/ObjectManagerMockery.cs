using Moq;
using Moq.Language;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class ObjectManagerMockery
    {
        private Mock<IObjectManager> _objectManagerMock;
        private ISetupSequentialResult<ZMachineObject> _getObjectSequence;

        public ObjectManagerMockery()
        {
            _objectManagerMock = new Mock<IObjectManager>();
            _objectManagerMock
                .Setup(o => o.GetObjectName(It.IsAny<ushort>()))
                .Returns("");
            _getObjectSequence = _objectManagerMock
                .SetupSequence(m => m.GetObject(It.IsAny<ushort>()));

        }

        public IObjectManager Object => _objectManagerMock.Object;


        public ObjectManagerMockery SetupGetObjectAddress(ushort returnValue)
        {
            _objectManagerMock.Setup(m
                    => m.GetObjectAddress(It.IsAny<ushort>())
                )
                .Returns(returnValue);

            return this;
        }

        public ObjectManagerMockery SetupSequenceGetObject(ZMachineObject obj)
        {
            _getObjectSequence.Returns(obj);
            return this;
        }

        public ObjectManagerMockery SetupGetObjectNumber(ushort returnValue)
        {
            _objectManagerMock.Setup(m
                    => m.GetObjectNumber(It.IsAny<ushort>())
                )
                .Returns(returnValue);

            return this;
        }
        public ObjectManagerMockery SetupGetObjectParent(ushort returnValue)
        {
            _objectManagerMock.Setup(m
                    => m.GetObjectParent(It.IsAny<ushort>())
                )
                .Returns(returnValue);

            return this;
        }
    }
}