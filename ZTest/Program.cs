using System;
using System.Collections.Generic;
using System.IO;

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

            CheckFilesExists(playerFile, programFile, testFile);
            _quietMode = args.Length == 3;

            var zMachineTestScript = new ZMachineTestScript(testFile);

            using var zPlayer = new TestableZPlayerProcess(playerFile, programFile);
            
            var failedExpectation = RunTest(zPlayer, zMachineTestScript);

            zPlayer.Close();

            ShowFinalMessage(programFile, testFile, failedExpectation);
        }

        private static string RunTest(TestableZPlayerProcess testableZPlayer,
            ZMachineTestScript zMachineTestScript)
        {
            var scriptLines = zMachineTestScript.Lines;
            string failedExpectation = string.Empty;
            testableZPlayer.Start();

            var lastCommand = string.Empty;

            if (_quietMode) Console.WriteLine("QUIET mode...");

            var lastOutput = testableZPlayer.CaptureOutputUntilTheNextCommandRequest();
            EchoToConsole(lastOutput);

            var scriptLineIdx = 0;
            do
            {
                var scriptLine = scriptLines[scriptLineIdx];

                if (scriptLine.HasCommand)
                {
                    testableZPlayer.ExecuteCommand(scriptLine.Command);
                    EchoToConsole($"{scriptLine.Command}\n");

                    lastCommand = scriptLine.Command;
                    lastOutput = testableZPlayer.CaptureOutputUntilTheNextCommandRequest();
                    EchoToConsole(lastOutput);
                }

                if (!scriptLine.MeetsExpectation(lastOutput))
                {
                    failedExpectation = CreateFailureMessage(lastCommand, scriptLine, lastOutput);
                    break;
                }

                scriptLineIdx++;

                ShowProgress(scriptLine);
            } while (scriptLineIdx < scriptLines.Count);

            Console.WriteLine($"\nCommand Log: {LogExecutedCommands(testableZPlayer.ExecutedCommands, zMachineTestScript.ScriptFile)}");

            return failedExpectation;
        }

        private static string LogExecutedCommands(IEnumerable<string> executedCommands, string scriptFile)
        {
            var filename = Path.GetFileName($"{scriptFile}.cmdlog");
            File.WriteAllLines(filename, executedCommands);
            return filename;
        }

        private static void ShowFinalMessage(string programFile, string testFile, string failedExpectation)
        {
            Console.WriteLine();
            if (!string.IsNullOrEmpty(failedExpectation))
            {
                ConsoleX.ColouredWriteLine("**FAILED**", ConsoleColor.Red, ConsoleColor.Yellow);
                Console.WriteLine(failedExpectation);
                Environment.ExitCode = -1;
            }
            else
            {
                ConsoleX.ColouredWriteLine($"SUCCESS", ConsoleColor.Green, ConsoleColor.Black);
                Environment.ExitCode = 0;
            }
            Console.WriteLine($"{programFile} tested using {testFile}");
        }

        private static void ShowProgress(CommandExpects testItem)
        {
            if (_quietMode)
            {
                Console.CursorLeft = 0;
                Console.Write($"Test line: {testItem.LineNo:D5}");
            }
        }

        private static void EchoToConsole(string lastOutput)
        {
            if (!_quietMode) Console.Write(lastOutput);
        }

        private static string CreateFailureMessage(string lastCommand, CommandExpects testItem, string lastOutput)
        {
            var failureMessage =
                $"Last command ('{lastCommand}') expected response from line [{testItem.LineNo}] ('{testItem.Expectation}')!";

            if (_quietMode) failureMessage += $"\nOutput was:\n{lastOutput}";
            return failureMessage;
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
    }
}
