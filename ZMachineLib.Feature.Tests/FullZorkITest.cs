﻿using NUnit.Framework;

namespace ZMachineLib.Feature.Tests
{
    
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
        }
    }
}