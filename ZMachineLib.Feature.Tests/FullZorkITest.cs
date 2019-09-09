﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Moq;
using NUnit.Framework;
using Shouldly;
using ZMachineLib;
using ZMachineLib.Operations;

namespace Tests
{
    [Explicit]
    public class FullZorkITest
    {
        private Mock<IZMachineIO> _zMachineIo;
        private List<string> _outputs;

        [SetUpAttribute]
        public void Setup()
        {
            _zMachineIo = new Mock<IZMachineIO>();
            _outputs = new List<string>();
        }

        [Test]

        public void Should_complete_zork_1_with_349_points()
        {
            var machine = new ZMachine2(_zMachineIo.Object);

            SetupInputs("zork1.349.txt");

            machine.LoadFile(File.OpenRead(@"zork1.dat"));
            Should.NotThrow(() => machine.Run());

            VerifyOutput(
                "349"
            );
        }


        private void VerifyOutput(params string[] commands)
        {
            foreach (var command in commands)
            {
                _zMachineIo.Verify(io
                    => io.Print(It.Is<string>(s => s.Contains(command))));
            }
        }
        private void SetupInputs(string zork1Txt)
        {
            var setupInputs = _zMachineIo.SetupSequence(io => io.Read(It.IsAny<int>()));

            var text = File.OpenText(zork1Txt).ReadToEnd();
            var lines = new StringReader(text);
            var line = lines.ReadLine();
            while(line != null)
            {
                if (line.Trim().StartsWith(">"))
                {
                    var cmd = line.Substring(1).Trim();
                    if (!string.IsNullOrWhiteSpace(cmd))
                    {
                        setupInputs.Returns(() =>
                        {
                            TestContext.WriteLine($"Executing: {cmd}");
                            return cmd;
                        });
                    }
                }

                line = lines.ReadLine();
            }

            setupInputs.Returns("quit");
            setupInputs.Returns("y");
        }
    }
}