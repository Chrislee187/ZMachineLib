using System;
using System.IO;
using Moq;
using Shouldly;
using ZMachineLib.Operations;

namespace ZMachineLib.Feature.Tests
{
    public class FeatureTestsBase
    {
        private Mock<IZMachineIo> _zMachineIo;
        private ZMachine2 Machine;
        protected ZMachineFeatureTester Feature;

        protected void ShouldRunToCompletion(string zMachineDataFile)
        {
            Should.NotThrow((Action) (() => Machine.RunFile(File.OpenRead(zMachineDataFile))));
            Feature.Verify();
        }

        protected void BaseSetup()
        {
            _zMachineIo = new Mock<IZMachineIo>();

            Feature = new ZMachineFeatureTester(_zMachineIo);
            Machine = new ZMachine2(_zMachineIo.Object);
        }
    }
}