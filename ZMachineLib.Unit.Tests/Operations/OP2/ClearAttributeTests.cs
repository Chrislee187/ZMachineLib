using Moq;
using NUnit.Framework;
using ZMachineLib.Content;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    public class ClearAttributeTests : OperationsTestsBase
    {
        private ClearAttribute _op;
        
        [SetUp]
        public void SetUp()
        {
            Setup();
            _op = new ClearAttribute(MemoryMock);
            MockPeekNextByte(_op);
        }

        [NUnit.Framework.Test]
        public void Should_store_attribute_on_object()
        {
            ushort obj = 1234;
            ushort attribute = 0x02;
            var args = new OpArgBuilder()
                .WithValue(obj)
                .WithValue(attribute)
                .Build();
            var zObj = new Mock<IZMachineObject>();

            ObjectTreeMockery.SetupGetIndexerReturns(zObj.Object);
//            ObjectManagerMockery.SetupSequenceGetObject(zObj.Object);

            _op.Execute(args);
            
            zObj.Verify(o => o.ClearAttribute(It.Is<ushort>(v => v == attribute)));

        }
    }
}