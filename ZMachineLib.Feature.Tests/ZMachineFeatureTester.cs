using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using Moq.Language;
using NUnit.Framework;

namespace ZMachineLib.Feature.Tests
{
    public class ZMachineFeatureTester
    {
        private readonly ISetupSequentialResult<string> _inputSequence;
        private readonly List<string> _outputSequence;
        private readonly Mock<IUserIo> _zMachineIo;

        public ZMachineFeatureTester(Mock<IUserIo> zMachineIo)
        {
            _zMachineIo = zMachineIo;

            _inputSequence = _zMachineIo.SetupSequence(io => io.Read(It.IsAny<int>()));
            _outputSequence = new List<string>();
        }

        public void Execute(string command, string outputContains = "")
        {                        // NOTE: Output text can be split across multiple calls to print mechansims

            if (!string.IsNullOrEmpty(command))
            {
                _inputSequence.Returns(command);
                TestContext.WriteLine($"\nExecuting: {command}\nExpecting:");
            }

            if (!string.IsNullOrWhiteSpace(outputContains))
            {
                foreach (var word in outputContains.Split(' '))
                {
                    var santise = Santise(word);
                    _outputSequence.Add(santise);
                    TestContext.Write($"{santise} ");
                }
            }
        }

        private string Santise(string word) 
            => new string(word.Where(c 
                => !".,:;#".Contains(c)
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
        public void Quit()
        {
            Execute("", "");
            Execute("quit", "wish to leave");
            Execute("Y");
        }

        public void Restart()
        {
            Execute("restart", "restart?");
            Execute("Y", "Restarting");
        }
        public void ExpectAdditionalOutput(params string[] commands)
        {
            _outputSequence.AddRange(commands);
        }


        public void Verify()
        {
            foreach (var outputContains in _outputSequence)
            {
                _zMachineIo.Verify(io
                    => io.Print(It.Is<string>(s => s.Contains(outputContains))));
            }
        }
    }
}