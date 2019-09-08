using System.IO;
using ZMachineLib;
using ZMachineLib.Operations;

namespace ConsoleZMachine
{
	class Program
	{
		static void Main(string[] args)
		{
			var zMachine = new ZMachine2(new ConsoleIO());

			FileStream fs = File.OpenRead(@"zork1.dat");
			zMachine.LoadFile(fs);
			zMachine.Run();
		}
	}
}
