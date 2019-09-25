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