using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using Moq.Language;
using NUnit.Framework;
using ZMachineLib.Content;

namespace ZMachineLib.Feature.Tests
{
    public class ZMachineFeatureTester
    {
        private readonly ISetupSequentialResult<string> _inputSequence;
        private readonly List<string> _outputStrings;
        private readonly List<string> _commandStrings;

        private int _commandIndex;
        private readonly StringBuilder _outputBetweenCommands;
        private string _lastCommand;

        public ZMachineFeatureTester(Mock<IUserIo> zMachineIo)
        {
            _inputSequence = zMachineIo
                .SetupSequence(io => io.Read(It.IsAny<int>(), It.IsAny<IZMemory>()));

            zMachineIo.Setup(m => m.Print(It.IsAny<string>()))
                .Callback<string>(s =>
                {
                    _outputBetweenCommands.Append(s);
                });

            _outputStrings = new List<string>();
            _commandStrings = new List<string>();
            _outputBetweenCommands = new StringBuilder();
        }

        public ZMachineFeatureTester Execute(string command, string outputContains = "")
        {                        
            _commandStrings.Add(command);
            _outputStrings.Add(outputContains);

            _inputSequence.Returns(HandleNextInputOutputPass);

            return this;
        }

        private string HandleNextInputOutputPass()
        {
            var commandString = _commandStrings[_commandIndex];
            do
            {
                var expectedText = _outputStrings[_commandIndex];

                if (!string.IsNullOrEmpty(commandString))
                {
                    Console.WriteLine($"{commandString}");
                }

                if (!string.IsNullOrEmpty(expectedText))
                {
                    CheckExpectation(expectedText);
                }

                commandString = _commandStrings[++_commandIndex];
            } while (string.IsNullOrEmpty(commandString));

            Console.Write($"{_outputBetweenCommands}");

            _lastCommand = commandString;
            _outputBetweenCommands.Clear();

            return commandString;
        }

        private void CheckExpectation(string expectedText)
        {
            var lastExpectationMet = _outputBetweenCommands
                .ToString()
                .Contains(expectedText);

            if (!lastExpectationMet)
            {
                Console.WriteLine("\n\n\nCommand Log:");
                Console.WriteLine(
                    string.Join('\n', _commandStrings
                        .ToArray()
                        .AsSpan(0, _commandIndex)
                        .ToArray()
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToArray())
                );

                var customMessage =
                    $"Last command ('{_lastCommand}') expected a response containing '{expectedText}'!\nResponse:\n" +
                    $"{_outputBetweenCommands}";
                Assert.Fail(customMessage);
            }
        }

        public void SetupInputs(string textFile)
        {
            var text = File.OpenText(textFile).ReadToEnd();
            var lines = new StringReader(text);
            var line = lines.ReadLine();
            var cmd = "";
            while (line != null)
            {
                if (!line.StartsWith('#'))
                {
                    if (line.Trim().StartsWith(">"))
                    {
                        if (!string.IsNullOrEmpty(cmd))
                        {
                            Execute(cmd);
                        }
                        cmd = line.Substring(1).Trim();
                    }
                    else
                    {
                        var result = line.Trim();

                        Execute(cmd, result);

                        cmd = string.Empty;
                    }
                }

                line = lines.ReadLine();
            }
        }

        public void Quit(int? score = null)
        {
            var t = Execute("quit");
            if (score != null)
                t.Expect($"Your score is {score}");

            t.Expect("wish to leave")
                .Execute("Y");
        }

        public void Restart()
        {
            Execute("restart", "restart?");
            Execute("Y", "Restarting");
        }

        public ZMachineFeatureTester Expect(params string[] commands)
        {
            foreach (var command in commands)
            {
                Execute("", command);
            }

            return this;
        }
    }
}