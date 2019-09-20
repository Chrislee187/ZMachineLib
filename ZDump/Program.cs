using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZMachineLib;

namespace ZDump
{
    class Program
    {
        static void Main(string[] args)
        {
            
            if (!args.Any() || !File.Exists(args[0]))
            {
                Console.Error.WriteLine("USAGE: ZDump: filename");
                Environment.ExitCode = -1;
                return;
            }

            var filename = args[0];

            var bytes = Read(File.OpenRead(filename));

            var contents = new ZMachineContents(bytes);

            WriteContents(filename, contents);

        }

        private static void WriteContents(string filename, ZMachineContents contents)
        {
            WriteHeading("Header");
            WriteHeader(filename, contents);

            WriteAbbreviations(contents.Abbreviations);
            WriteDictionary(contents);
        }
        private static void WriteDictionary(ZMachineContents contents)
        {
            WriteHeading("Dictionary");

            var d = contents.Dictionary;

            var pairs = new Dictionary<string, string>();

            pairs.Add($"InputCodes", $"{Format.ByteArray(d.InputCodes, false)} {Format.CharArray(d.InputCodes, false)}");

            Console.WriteLine(Format.TwoColumn(pairs, 25));
            WriteSubHeading("Words");

            Console.WriteLine(string.Join(", ", d.Words));
        }
        private static void WriteAbbreviations(ZAbbreviations abbrevs)
        {
            WriteHeading("Abbreviations");

            Console.WriteLine(string.Join("|", abbrevs.Abbreviations));
        }

        private static void WriteHeading(string header)
        {
            var origColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(header);
            Console.ForegroundColor = origColour;
        }
        private static void WriteSubHeading(string header)
        {
            var origColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(header);
            Console.ForegroundColor = origColour;
        }
        private static void WriteHeader(string filename, ZMachineContents contents)
        {
            var h = contents.Header;

            var pairs = new Dictionary<string, string>();

            pairs.Add($"File", Path.GetFileNameWithoutExtension(filename));
            pairs.Add($"ZMachine Version", $"{h.Version}");
            pairs.Add("Addresses", "");
            pairs.Add($"Program Counter:", Format.Word(h.ProgramCounter));
            pairs.Add($"High Memory", Format.Word(h.HighMemoryBaseAddress));
            pairs.Add($"Static Memory", Format.Word(h.StaticMemoryBaseAddress));
            pairs.Add($"Dictionary", Format.Word(h.Dictionary));
            pairs.Add($"Object Table", Format.Word(h.ObjectTable));
            pairs.Add($"Globals", Format.Word(h.Globals));
            pairs.Add($"Abbreviations Table", Format.Word(h.AbbreviationsTable));
            pairs.Add($"Flags1", Format.Flags((byte) h.Flags1));
            pairs.Add("Flags2", Format.Flags(h.Flags2));

            Console.WriteLine(Format.TwoColumn(pairs, 25));
        }


        private static byte[] Read(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }
    }
}
