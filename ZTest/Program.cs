using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Channels;

namespace ZTest
{
    class Program
    {
        private static bool _quietMode;

        static void Main(string[] args)
        {
            var programFile = args[0];
            var testFile = args[1];
            var playerFile = @"D:\Src\ZMachineLib\ZPlay\bin\Debug\netcoreapp3.0\zplay.exe";

            _quietMode = args.Length == 3;

            CheckFilesExists(playerFile, programFile, testFile);

            var testData = ReadTestFile(testFile);

            using var zPlayer = CreateRedirectedPlayerProcess(playerFile, programFile);
            zPlayer.Start();

            var testLine = 0;
            var lastLine = testData.Max(td => td.LineNo);
            var lastCommand = string.Empty;
            var failedExpectation = string.Empty;

            // Grab and show any startup output
            var lastOutput = ReadToNextCommandRequest(zPlayer.StandardOutput);

            if (_quietMode) Console.WriteLine("QUIET mode...");

            EchoToConsole(lastOutput);

            do
            {
                var testItem = testData[testLine];

                if (testItem.HasCommand)
                {
                    lastCommand = testItem.Command;

                    zPlayer.StandardInput.WriteLine(testItem.Command);

                    EchoToConsole($"{lastCommand}\n");

                    lastOutput = ReadToNextCommandRequest(zPlayer.StandardOutput);

                    EchoToConsole(lastOutput);
                }

                if (testItem.HasExpectation)
                {
                    var lastExpectationMet = true;

                    if (!string.IsNullOrEmpty(lastOutput))
                    {
                        lastExpectationMet = lastOutput.Contains(testItem.Expectation);
                    }

                    if (!lastExpectationMet)
                    {
                        var failureMessage =
                            $"Last command ('{lastCommand}') expected response from line [{testItem.LineNo}] ('{testItem.Expectation}')!";

                        if (_quietMode) failureMessage += $"\nOutput was:\n{lastOutput}";

                        failedExpectation = failureMessage;
                        break;
                    }
                }

                testLine++;

                if (_quietMode)
                {
                    Console.CursorLeft = 0;
                    Console.Write($"{testItem.LineNo:D5}");
                }

            } while (testLine < testData.Length);


            zPlayer.StandardInput.Close();
            zPlayer.StandardOutput.Close();
            zPlayer.Kill();

            Console.WriteLine();

            if (!string.IsNullOrEmpty(failedExpectation))
            {
                ConsoleX.ColouredWriteLine(ConsoleColor.Red, ConsoleColor.Yellow, "**FAILED**");
                Console.WriteLine(failedExpectation);
                Environment.ExitCode = -1;
            }
            else
            {
                ConsoleX.ColouredWriteLine(ConsoleColor.Green, ConsoleColor.Black, $"SUCCESS");
                Console.WriteLine($"{programFile} tested using {testFile}");
                Environment.ExitCode = 0;
            }

        }

        private static void EchoToConsole(string lastOutput)
        {
            if (!_quietMode) Console.Write(lastOutput);
        }

        private static Process CreateRedirectedPlayerProcess(string playerFile, string programFile)
        {
            return new Process
            {
                StartInfo =
                {
                    FileName = playerFile,
                    Arguments = programFile,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                }
            };
        }

        private static void CheckFilesExists(string playerFile, string programFile, string testFile)
        {
            void Exists(string fileName)
            {
                if (!File.Exists(fileName)) throw new FileNotFoundException("File not found.", fileName);
            }

            Exists(playerFile);
            Exists(programFile);
            Exists(testFile);
        }

        private static string ReadToNextCommandRequest(StreamReader playerOutputReader)
        {
            var playerOutput = "";
            var nextChar = (char) playerOutputReader.Read();
            while (nextChar != '>' && nextChar != 0xffff)
            {
                playerOutput += $"{nextChar}";
                nextChar = (char) playerOutputReader.Read();
            }

            playerOutput += $"{nextChar}";
            return playerOutput;
        }

        private static CommandExpects[] ReadTestFile(string zTextFile)
        {
            var text = File.OpenText(zTextFile).ReadToEnd();
            var lines = new StringReader(text);
            var line = lines.ReadLine();
            var cmd = "";

            var list = new List<CommandExpects>();
            var lineNo = 1;
            while (line != null)
            {
                if (!line.StartsWith('#'))
                {
                    if (line.Trim().StartsWith(">"))
                    {
                        if (!string.IsNullOrEmpty(cmd))
                        {
                            list.Add(new CommandExpects(cmd, "", lineNo));
                        }

                        cmd = line.Substring(1).Trim();
                    }
                    else
                    {
                        var result = line.Trim();

                        list.Add(new CommandExpects(cmd, result, lineNo));

                        cmd = string.Empty;
                    }
                }

                lineNo++;
                line = lines.ReadLine();
            }

            return list.ToArray();
        }
    }
}
