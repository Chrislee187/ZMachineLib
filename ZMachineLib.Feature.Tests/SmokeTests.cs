using System.Collections.Generic;
using System.IO;
using Moq;
using NUnit.Framework;
using Shouldly;
using ZMachineLib;
using ZMachineLib.Operations;

namespace Tests
{
    [Timeout(5000)]
    public class Zork1BasedSantityTests
    {
        private Mock<IZMachineIO> _zMachineIo;
        private List<string> _outputs;

        [SetUp]
        public void Setup()
        {
            _zMachineIo = new Mock<IZMachineIO>();
            _outputs = new List<string>();
        }

        [Test]

        public void Should_start_and_quit_zork1_without_error()
        {

            SetupInputSequence("quit", "y");
            var machine = new ZMachine2(_zMachineIo.Object);

            machine.LoadFile(File.OpenRead(@"zork1.dat"));
            Should.NotThrow(() => machine.Run());

            VerifyOutputContained(
                "ZORK I",
                "West of House"
            );
        }
        [Test]

        public void Should_move_north_then_south_without_error()
        {

            SetupInputSequence("n", "s", "quit", "y");
            var machine = new ZMachine2(_zMachineIo.Object);

            machine.LoadFile(File.OpenRead(@"zork1.dat"));
            Should.NotThrow(() => machine.Run());

            VerifyOutputContained(
                "ZORK I",
                "West of House",
                "North of House",
                "boarded"
            );
        }

        private void SetupInputSequence(params string[] commands)
        {
            var setup = _zMachineIo.SetupSequence(io => io.Read(It.IsAny<int>()));

            foreach (var command in commands)
            {
                setup.Returns(() => command);
            }
        }


        private void VerifyOutputContained(params string[] commands)
        {
            foreach (var command in commands)
            {
                _zMachineIo.Verify(io
                    => io.Print(It.Is<string>(s => s.Contains(command))));
            }
        }
    }
}