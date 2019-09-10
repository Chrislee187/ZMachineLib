using System.IO;
using ZMachineLib;
using ZMachineLib.Operations;

namespace ConsoleZMachine
{
	class Program
	{
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            RunNewMachine(@"zork3.z3");
        }

        private static void RunNewMachine(string filename)
        {
            var zMachine = new ZMachine2(new ConsoleIo());

            FileStream fs = File.OpenRead(filename);
            zMachine.RunFile(fs);
        }

        static void RunOriginalMachine(string filename)
        {
            var zMachine = new ZMachine(new ConsoleIo());

            FileStream fs = File.OpenRead(filename);
            zMachine.LoadFile(fs);
            zMachine.Run();
        }
	}
}
