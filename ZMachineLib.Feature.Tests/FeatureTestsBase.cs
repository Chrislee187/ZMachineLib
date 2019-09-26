using System.IO;
using Moq;
using Shouldly;

namespace ZMachineLib.Feature.Tests
{
    public class FeatureTestsBase
    {
        protected Mock<IUserIo> MockUserIo;
        private Mock<IFileIo> MockFileIo;
        private ZMachine2 _machine;
        protected ZMachineFeatureTester Feature;

        protected void ShouldRunToCompletion(string zMachineDataFile)
        {
            Should.NotThrow(() => _machine.RunFile(File.OpenRead(zMachineDataFile)));
            Feature.Verify();
        }

        protected void BaseSetup()
        {
            MockUserIo = new Mock<IUserIo>();
            MockFileIo = new Mock<IFileIo>();
            Feature = new ZMachineFeatureTester(MockUserIo);
            _machine = new ZMachine2(MockUserIo.Object, MockFileIo.Object);
        }
    }
}