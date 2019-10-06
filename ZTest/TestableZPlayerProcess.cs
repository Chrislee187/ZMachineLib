using System.Collections.Generic;
using System.Diagnostics;

namespace ZTest
{
    public class TestableZPlayerProcess : Process
    {
        private readonly List<string> _executedCommands = new List<string>();
        public IEnumerable<string> ExecutedCommands => _executedCommands;
        public TestableZPlayerProcess(string playerFile, string programFile)
        {
            StartInfo.FileName = playerFile;
            StartInfo.Arguments = programFile;
            StartInfo.UseShellExecute = false;
            StartInfo.RedirectStandardInput = true;
            StartInfo.RedirectStandardOutput = true;
        }
        public void ExecuteCommand(string testItemCommand)
        {
            StandardInput.WriteLine(testItemCommand);
            _executedCommands.Add(testItemCommand);
        }

        public string CaptureOutputUntilTheNextCommandRequest()
        {
            var playerOutput = "";
            var nextChar = (char)StandardOutput.Read();
            while (nextChar != '>' && nextChar != 0xffff)
            {
                playerOutput += $"{nextChar}";
                nextChar = (char)StandardOutput.Read();
            }

            playerOutput += $"{nextChar}";
            return playerOutput;
        }

        public new void Close()
        {
            StandardInput.Close();
            StandardOutput.Close();
            base.Close();
        }
    }
}