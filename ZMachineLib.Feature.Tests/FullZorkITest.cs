using System.IO;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ZMachineLib.Feature.Tests
{
    [Explicit("Very slow tests that verify lots of commands and result text")]
    public class FullZorkITest : FeatureTestsBase
    {
        private const string Zork3V3 = "zork1.z3";
        [SetUp]
        public void Setup()
        {
            BaseSetup();
        }

        [Test]

        public void Should_play_Zork_I()
        {
            Feature.SetupInputs("zork1.349.txt");
            Feature.Quit();

            ShouldRunToCompletion(Zork3V3);

//            VerifyOutput(
//                "349"
//            );
        }


        private void VerifyOutput(params string[] commands)
        {
            foreach (var command in commands)
            {
                _zMachineIo.Verify(io
                    => io.Print(It.Is<string>(s => s.Contains(command))));
            }
        }
    }
}