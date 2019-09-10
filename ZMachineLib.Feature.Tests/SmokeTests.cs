using System.IO;
using Moq;
using NUnit.Framework;
using Shouldly;
using Tests;
using ZMachineLib.Operations;

namespace ZMachineLib.Feature.Tests
{
    [Timeout(5000)]
    public class Zork1BasedSanityTests
    {
        private Mock<IZMachineIO> _zMachineIo;
        private ZMachineFeatureTester _zMachineFeature;
        private ZMachine2 _machine;

        [SetUp]
        public void Setup()
        {
            _zMachineIo = new Mock<IZMachineIO>();

            _zMachineFeature = new ZMachineFeatureTester(_zMachineIo);
            _machine = new ZMachine2(_zMachineIo.Object);
        }

        [Test]

        public void Should_start_and_quit_zork1_without_error()
        {
            ExpectZorkIStartText();
            _zMachineFeature.Quit();

            ShouldRunToCompletion(@"zork1.dat");
        }

        [Test]

        public void Should_move_north_then_south_without_error()
        {
            _zMachineFeature.Execute("n", "North of House");
            _zMachineFeature.Execute("s", "boarded");
            _zMachineFeature.Quit();

            ShouldRunToCompletion(@"zork1.dat");
        }

        [Test]
        public void Should_restart()
        {
            _zMachineFeature.Execute("n", "North of House");
            _zMachineFeature.Restart();
            _zMachineFeature.Quit();

            ShouldRunToCompletion(@"zork1.dat");
        }

        [Test]
        public void Should_save_and_load()
        {
            // TODO: Need seperate the Save/Load mechanism out from the IZMachineIO interface
            // to a seperate interface
        }

        private void ShouldRunToCompletion(string zMachineDataFile)
        {
            Should.NotThrow(() => _machine.RunFile(File.OpenRead(zMachineDataFile)));
            _zMachineFeature.Verify();
        }

        private void ExpectZorkIStartText()
        {
            _zMachineFeature.ExpectAdditionalOutput(
                "ZORK I",
                "West of House"
            );
        }
    }
}