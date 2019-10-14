using NUnit.Framework;

namespace ZMachineLib.Feature.Tests
{
    
    public class FullZorkITest : FeatureTestsBase
    {
//        private const string Zork3V3 = "zork1.z3";
        private const string Zork3V2 = @"\\NAS\nas\Vault\Infocom Files\zFiles\zork_1.z2";
        private const string Zork3V3 = @"\\NAS\nas\Vault\Infocom Files\zFiles\zork1.z3";
        [SetUp]
        public void Setup()
        {
            BaseSetup();
        }

        [Test, Explicit]

        public void Should_play_Zork_I_v2()
        {
            // TODO: V2 Zork1 doesn't support the #rand command and has slighlty different outputs
            Feature.SetupInputs("zork1.rand0.ztest");
            Feature.Quit();

            ShouldRunToCompletion(Zork3V2);
        }

        [Test]

        public void Should_play_Zork_I_v3()
        {
            Feature.SetupInputs("zork1.rand0.ztest");
            Feature.Quit();

            ShouldRunToCompletion(Zork3V3);
        }
    }
}