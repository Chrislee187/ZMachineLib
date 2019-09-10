using System.IO;
using ZMachineLib.Operations;

namespace ConsoleZMachine
{
	class Program
	{
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
		{
			var zMachine = new ZMachine2(new ConsoleIo());

			FileStream fs = File.OpenRead(@"zork1.dat");
			zMachine.RunFile(fs);
		}
	}
}
