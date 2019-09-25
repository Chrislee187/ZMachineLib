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
            ExpectZorkIStartText();
            Feature.Execute("n", "North of House");
            Feature.Restart();
            ExpectZorkIStartText();
            Feature.Quit();

            ShouldRunToCompletion(TestFile);
        }

        [Test, Ignore("Need an explicit FileIO to properly test this")]
        public void Should_save_and_load()
        {
            ExpectZorkIStartText();
            Feature.Execute("n", "North of House");
            Feature.Execute("save", "saved");

            Feature.Execute("restore", "Ok.");
            Feature.Quit();

            ShouldRunToCompletion(TestFile);

        }

        [Test]
        public void Should_drink_the_bottle_of_water()
        {
            Feature.Execute("s");
            Feature.Execute("e");
            Feature.Execute("open window");
            Feature.Execute("go thru window");
            Feature.Execute("get bottle");
            Feature.Execute("open bottle", "Opened.");
            Feature.Execute("drink water", "Thank you very much.");
            Feature.Quit();

            ShouldRunToCompletion(TestFile);
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