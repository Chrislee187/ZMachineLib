using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ZMachineLib;

namespace ZPlay
{
    class Program
    {
        private static IConfiguration _config;
        private static ZMachine2 _zMachine2;

        static void Main(string[] args)
        {
            _config = Config.Get();
            string zFile = string.Empty;
            zFile = GetZFileToPlay(args, zFile);

            if (!File.Exists(zFile))
            {
                Abort("USAGE: ZPlay <zfile>|<zFileDir>");
            }

            RunZFile(zFile);
//            RunZFileWithoutInterrupts(zFile);

        }
        private static string GetZFileToPlay(string[] args, string zFile)
        {
            if (args.Any())
            {
                if (File.Exists(args[0]))
                {
                    zFile = args[0];
                }
                else if (Directory.Exists(args[0]))
                {
                    zFile = SelectFile(args[0]);
                }
            }
            else
            {
                zFile = SelectFile(_config["zFileDir"]);
            }

            return zFile;
        }

        /// <summary>
        /// Simple test for the non-interrupt version
        /// </summary>
        /// <param name="zFile"></param>
        // ReSharper disable once UnusedMember.Local
        private static void RunZFileWithoutInterrupts(string zFile)
        {
            var progId = Path.GetFileNameWithoutExtension(zFile);
            _zMachine2 = new ZMachine2(
                new UserIo(progId),
                new ConsoleFileIo(progId),
                LoggerSetup.Create<ZMachine2>()
            );
            _zMachine2.RunFile(File.OpenRead(zFile), false);

            do
            {
                var input = Console.ReadLine();
                _zMachine2.ContinueTillNextRead(input);

            } while (_zMachine2.Running);
        }
        private static void RunZFile(string zFile)
        {
            var progId = Path.GetFileNameWithoutExtension(zFile);
            new ZMachine2(
                new UserIo(progId),
                new ConsoleFileIo(progId),
                LoggerSetup.Create<ZMachine2>()
            ).RunFile(zFile);
        }

        private static void Abort(string s)
        {
            Console.WriteLine(s);
            Environment.Exit(-1);
        }

        private static string SelectFile(string fromFolder)
        {
            var zFiles = Directory.GetFiles(fromFolder, "*.z?", SearchOption.TopDirectoryOnly);
            var files = zFiles.Concat(Directory.GetFiles(fromFolder, "*.dat", SearchOption.TopDirectoryOnly)).OrderBy(f => f).ToArray();

            var idx = 1;
            Console.WriteLine("0 - QUIT");
            foreach (var datFile in files)
            {
                Console.WriteLine($"{idx++} - {Path.GetFileNameWithoutExtension(datFile)}");
            }

            Console.Write("Select a file: ");
            var selection = Console.ReadLine();

            if (int.TryParse(selection, out int result))
            {
                if(result != 0) return files[result - 1];
            }

            return string.Empty;
        }
    }
}
