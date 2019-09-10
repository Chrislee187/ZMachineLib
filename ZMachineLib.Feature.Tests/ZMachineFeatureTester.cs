using System.Collections.Generic;
using Moq;
using Moq.Language;

namespace ZMachineLib.Feature.Tests
{
    public class ZMachineFeatureTester
    {
        private readonly ISetupSequentialResult<string> _inputSequence;
        private readonly List<string> _outputSequence;
        private readonly Mock<IZMachineIo> _zMachineIo;

        public ZMachineFeatureTester(Mock<IZMachineIo> zMachineIo)
        {
            _zMachineIo = zMachineIo;

            _inputSequence = _zMachineIo.SetupSequence(io => io.Read(It.IsAny<int>()));
            _outputSequence = new List<string>();
        }

        public void Execute(string command, string outputContains = "")
        {
            _inputSequence.Returns(command);

            if (!string.IsNullOrWhiteSpace(outputContains))
            {
                _outputSequence.Add(outputContains);
            }
        }

        public void Quit()
        {
            Execute("quit", "wish to leave");
            Execute("y");
        }

        public void Restart()
        {
            Execute("restart", "restart?");
            Execute("y", "Restarting");
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