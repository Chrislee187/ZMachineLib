using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using ZMachineLib;

namespace ZPlay
{
    class Program
    {
        static void Main(string[] args)
        {
//            var configuration = InitConfig();
//            Console.WriteLine(configuration["ProgramDir"]);

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("USAGE: ZPlay <zfile.z?>");
            }

            var filename = args[0];
            var gameName = Path.GetFileNameWithoutExtension(filename);
            var machine = new ZMachine2(new ConsoleIo(), new FileIo(gameName));
            machine.RunFile(args[0]);
        }

        private static IConfigurationRoot InitConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            return configuration;
        }
    }
}
