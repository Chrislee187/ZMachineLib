using System.IO;
using Moq;
using Shouldly;

namespace ZMachineLib.Feature.Tests
{
    public class FeatureTestsBase
    {
        protected Mock<IUserIo> _zMachineIo;
        private Mock<IFileIo> _fileIo;
        private ZMachine2 _machine;
        protected ZMachineFeatureTester Feature;

        protected void ShouldRunToCompletion(string zMachineDataFile)
        {
            Should.NotThrow(() => _machine.RunFile(File.OpenRead(zMachineDataFile)));
            Feature.Verify();
        }

        protected void BaseSetup()
        {
            _zMachineIo = new Mock<IUserIo>();
            _fileIo = new Mock<IFileIo>();
            Feature = new ZMachineFeatureTester(_zMachineIo);
            _machine = new ZMachine2(_zMachineIo.Object, _fileIo.Object);
        }
    }
}