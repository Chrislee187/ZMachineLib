using Moq;
using NUnit.Framework;
using ZMachineLib.Content;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Unit.Tests.Operations.OP2
{
    /// <summary>
    /// 2OP:12 C clear_attr object attribute
    /// Make object not have the attribute numbered attribute
    /// </summary>
    public class ClearAttributeTests : OperationsTestsBase<ClearAttribute>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [NUnit.Framework.Test]
        public void Should_clear_attribute()
        {
            ushort obj = 1234;
            ushort attribute = 0x02;
            var args = new OperandBuilder()
                .WithArg(obj)
                .WithArg(attribute)
                .Build();
            
            var zObj = new Mock<IZMachineObject>();
            var zAttributes = new Mock<IZAttributes>();
            zObj.Setup(z => z.Attributes).Returns(zAttributes.Object);
            Mockery.SetNextObject(zObj.Object);

            Operation.Execute(args);

            zAttributes.Verify(o => o.ClearAttribute((byte) It.Is<byte>(v => v == attribute)));

        }
    }
}