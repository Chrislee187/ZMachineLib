using Moq;
using ZMachineLib.Content;
using ZMachineLib.Extensions;
using ZMachineLib.Managers;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class OperationsMockery
    {
        private readonly VariableManagerMockery _vmMockery;
        private readonly ObjectTreeMockery _otMockery;
        private readonly Mock<IZMemory> _memoryMock;

        public IZMemory Memory => _memoryMock.Object;

        public OperationsMockery(VariableManagerMockery vmMockery,
            ObjectTreeMockery otMockery)
        {
            _otMockery = otMockery;
            _vmMockery = vmMockery;
            _memoryMock = new Mock<IZMemory>();

            _memoryMock
                .SetupGet(m => m.VariableManager)
                .Returns(vmMockery.Mock.Object);

            _memoryMock
                 .SetupGet(m => m.Manager)
                .Returns(new Mock<IMemoryManager>().Object);

            _memoryMock
                .SetupGet(m => m.ObjectTree)
                .Returns(_otMockery.Object);

        }
        public OperationsMockery VariableRetrieved(ushort expected)
        {
            _vmMockery.UShortWasRetrieved(expected);
            return this;
        }

        public OperationsMockery ResultStored(ushort expected)
        {
            _vmMockery.UShortWasStored(expected);
            return this;
        }
        public OperationsMockery ResultStored(short expected)
            => ResultStored((ushort)expected);

        public OperationsMockery ResultStored(byte expected)
        {
            _vmMockery.ByteWasStored(expected);
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

    }


    public class VariableManagerMockery
    {
        public readonly Mock<IVariableManager> Mock;

        public VariableManagerMockery()
        {
            Mock = new Mock<IVariableManager>();
        }

        public IVariableManager Object => Mock.Object;
        

        public VariableManagerMockery UShortWasRetrieved(ushort returnValue)
        {
            Mock.Setup(m
                    => m.GetUShort(
                        It.IsAny<byte>(),
                        It.IsAny<bool>())
                )
                .Returns(returnValue);

            return this;
        }

        public VariableManagerMockery UShortWasStored(ushort expectedValue)
        {
            Mock.Verify(m => m.Store(
                It.IsAny<byte>(),
                It.Is<ushort>(v => v == expectedValue),
                It.Is<bool>(b => b)), Times.Once);

            return this;
        }
        public VariableManagerMockery ByteWasStored(byte expectedValue)
        {
            Mock.Verify(m => m.Store(
                It.IsAny<byte>(),
                It.Is<byte>(v => v == expectedValue)
                ), Times.Once);

            return this;
        }
    }
}