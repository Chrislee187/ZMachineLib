using Moq;
using ZMachineLib.Content;

namespace ZMachineLib.Unit.Tests.Operations
{
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