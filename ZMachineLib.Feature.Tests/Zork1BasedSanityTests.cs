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
            Feature.Quit(0);

            ShouldRunToCompletion(TestFile);
        }

        [Test]

        public void Should_move_north_then_south_without_error()
        {
            ExpectZorkIStartText();
            Feature.Execute("n", "North of House");
            Feature.Execute("s", "boarded");
            Feature.Execute("look", "North of House");
            Feature.Quit(0);

            ShouldRunToCompletion(TestFile);
        }

        [Test]
        public void Should_restart()
        {
            ExpectZorkIStartText();
            Feature
                .Execute("n", "North of House")
                .Restart();
            ExpectZorkIStartText();
            Feature.Quit();

            ShouldRunToCompletion(TestFile);
        }

        [Test, Ignore("Need an explicit FileIO to properly test this")]
        public void Should_save_and_load()
        {
            ExpectZorkIStartText();
            Feature
                .Execute("n", "North of House")
                .Execute("save", "saved")
                .Execute("restore", "Ok.")
                .Quit();

            ShouldRunToCompletion(TestFile);
        }

        [Test]
        public void Should_drink_the_bottle_of_water()
        {
            ExpectZorkIStartText();
            Feature.Execute("s")
                .Execute("e")
                .Execute("open window")
                .Execute("go thru window")
                .Execute("get bottle")
                .Execute("i", "A glass bottle")
                .ExpectAdditionalOutput("A quantity of water")
                .Execute("open bottle", "Opened.")
                .Execute("drink water", "Thank you very much.")
                .Execute("i", "A glass bottle")
                .Execute("drink water", "You can't see any water here!")
                .Quit(10);

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