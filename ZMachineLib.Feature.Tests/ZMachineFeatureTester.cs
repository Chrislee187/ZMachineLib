using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Castle.Core.Internal;
using Microsoft.VisualBasic.CompilerServices;
using Moq;
using Moq.Language;
using Shouldly;

namespace ZMachineLib.Feature.Tests
{
    public class ZMachineFeatureTester
    {
        private readonly ISetupSequentialResult<string> _inputSequence;
        private readonly List<string> _outputStrings;
        private readonly List<string> _commandStrings;
        private readonly Mock<IUserIo> _zMachineIo;
        private string _lastCommand;

        private string _lastExpectation;

        private int _commandIndex = 0;
        private StringBuilder _outputBetweenCommands;
        private bool _firstRead;

        public ZMachineFeatureTester(Mock<IUserIo> zMachineIo)
        {
            _zMachineIo = zMachineIo;

            _inputSequence = _zMachineIo
                .SetupSequence(io => io.Read(It.IsAny<int>()));

            _zMachineIo.Setup(m => m.Print(It.IsAny<string>()))
                .Callback<string>(s =>
                {
                    _outputBetweenCommands.Append(s);
                });

            _outputStrings = new List<string>();
            _commandStrings = new List<string>();
            _outputBetweenCommands = new StringBuilder();
        }

        public ZMachineFeatureTester Execute(string command, string outputContains = "")
        {                        // NOTE: Output text can be split across multiple calls to print mechansims

            _commandStrings.Add(command);
            _outputStrings.Add(outputContains);
                _firstRead = true;
                _inputSequence.Returns(() =>
                {

                    var commandString = _commandStrings[_commandIndex];
                    do
                    {
                        var outputString = _outputStrings[_commandIndex];

                        if (!string.IsNullOrEmpty(commandString))
                        {
                            Console.WriteLine($"{commandString}");
                        }

                        if (!string.IsNullOrEmpty(outputString))
                        {
                            var lastExpectationMet = _outputBetweenCommands
                                .ToString()
                                .Contains(outputString);

                            if (!lastExpectationMet)
                            {
                                Console.WriteLine(
                                    string.Join('\n', _commandStrings
                                        .ToArray()
                                        .AsSpan(0, _commandIndex)
                                        .ToArray()
                                        .Where(s => !string.IsNullOrWhiteSpace(s))
                                        .ToArray())
                                    );
                            }

                            lastExpectationMet.ShouldBeTrue(
                                $"Last expectation of '{outputString}' was not found in last output recorded:\n" +
                                $"{_outputBetweenCommands}");
                        }
                        _lastCommand = commandString;

                        commandString = _commandStrings[++_commandIndex];
                    } while (string.IsNullOrEmpty(commandString));
                    Console.Write($"{_outputBetweenCommands}\n");


                    _outputBetweenCommands.Clear();
                    return commandString;
                });

                return this;
        }

        private string Santise(string word) 
            => new string(word.Where(c 
                => !".,:;".Contains(c)
                ).ToArray());

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

        public void SetupQuickInputs(string textFile)
        {
            var text = File.OpenText(textFile).ReadToEnd();
            var lines = new StringReader(text);
            var line = lines.ReadLine();
            while (line != null)
            {
                if (!line.StartsWith("//"))
                {
                    Execute(line.Trim());
                }

                line = lines.ReadLine();
            }
        }
        public void Quit(int? score = null)
        {
            var t = Execute("quit");
            if (score != null)
                t.ExpectAdditionalOutput($"Your score is {score}");

            t.ExpectAdditionalOutput("wish to leave")
                .Execute("Y");
        }

        public void Restart()
        {
            Execute("restart", "restart?");
            Execute("Y", "Restarting");
        }
        public ZMachineFeatureTester ExpectAdditionalOutput(params string[] commands)
        {
            foreach (var command in commands)
            {
                Execute("", command);
            }

            return this;
        }


        public void Verify()
        {
            foreach (var outputContains in _outputStrings)
            {
                _zMachineIo.Verify(io
                    => io.Print(It.Is<string>(s => s.Contains(outputContains))));
            }
        }
    }
}