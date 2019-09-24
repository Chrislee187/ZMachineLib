using Moq;
using Moq.Language;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class ObjectManagerMockery
    {
        private readonly Mock<IObjectManager> _objectManagerMock;
        private readonly ISetupSequentialResult<IZMachineObject> _getObjectSequence;

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

        public ObjectManagerMockery SetupSequenceGetObject(IZMachineObject obj)
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

        public ObjectManagerMockery VerifyGetObject(ushort obj)
        {
            _objectManagerMock
                .Verify(m
                        => m.GetObject(It.Is<ushort>(v => v == obj))
                    , Times.Once);

            return this;
        }
    }

    public class ObjectTreeMockery
    {
        public IZObjectTree Object => _objectTreeMock.Object;
        private readonly Mock<IZObjectTree> _objectTreeMock;

        public ObjectTreeMockery()
        {
            _objectTreeMock = new Mock<IZObjectTree>();
            _objectTreeMock
                .Setup(m => m[It.IsAny<ushort>()])
                .Returns(new ZMachineObjectBuilder().Build());
        }

        public ObjectTreeMockery SetupGetIndexerReturns(IZMachineObject zObj)
        {
            _objectTreeMock
                .Setup(m => m[It.IsAny<ushort>()])
                .Returns(zObj);

            return this;
        }


    }
}