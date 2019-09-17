using NUnit.Framework;

namespace ZMachineLib.Feature.Tests
{
    [Timeout(5000)]
    public class Zork1BasedSanityTests : FeatureTestsBase
    {
        private const string TestFile = "zork1.z3";

        [SetUp]
        public void Setup()
        {
            BaseSetup();
        }

        [Test]

        public void Should_start_and_quit_zork1_without_error()
        {
            ExpectZorkIStartText();
            Feature.Quit();

            ShouldRunToCompletion(TestFile);
        }

        [Test]

        public void Should_move_north_then_south_without_error()
        {
            Feature.Execute("n", "North of House");
            Feature.Execute("s", "boarded");
            Feature.Quit();

            ShouldRunToCompletion(TestFile);
        }

        [Test]
        public void Should_restart()
        {
            Feature.Execute("n", "North of House");
            Feature.Restart();
            Feature.Quit();

            ShouldRunToCompletion(TestFile);
        }

        [Test]
        public void Should_save_and_load()
        {
            Assert.Inconclusive("TODO");
        }

        [Test]
        public void Should_drink_the_bottle_of_water()
        {
            // This tests exercises some additional operations that simple moving around do not
            // i.e. RemoveObj is called when the water is drunk
            Assert.Inconclusive("TODO");
        }

        private void ExpectZorkIStartText()
        {
            Feature.ExpectAdditionalOutput(
                "ZORK I",
                "West of House"
            );
        }
    }
}