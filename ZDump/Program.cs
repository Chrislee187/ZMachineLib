using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZMachineLib;
using ZMachineLib.Content;

namespace ZDump
{
    class Program
    {
        static void Main(string[] args)
        {
            if (CheckArguments(args)) return;

            var filename = args[0];

            var bytes = Read(File.OpenRead(filename));

            if (CheckStoryVersion(bytes)) return;

            var contents = new ZMemory(bytes, null);

            WriteContents(filename, contents);
        }

        private static bool CheckArguments(string[] args)
        {
            // TODO: Pull in YACLAP and add some options to control whats is output
            // -header, -abbreviations, -dictionary, -objects s, t, v
            // s (simple)  = Object Number & Name on a single line
            // t (terse)   = Object Number & Name, Parent, Child, Sibling addresses on a single line
            // v (verbose) = Full, multi-line per object, output
            if (!args.Any() || !File.Exists(args[0]))
            {
                Console.Error.WriteLine("USAGE: ZDump: storyfile");
                Environment.ExitCode = -1;
                return true;
            }

            return false;
        }

        private static bool CheckStoryVersion(byte[] bytes)
        {
            if (bytes[0] > 3)
            {
                Console.Error.WriteLine(" >V3 Story files not currently supported");
                Environment.ExitCode = -1;
                return true;
            }

            return false;
        }

        private static void WriteContents(string filename, ZMemory contents)
        {
            WriteHeading("Header");
            WriteHeader(filename, contents);

            WriteAbbreviations(contents.Abbreviations);
            WriteDictionary(contents);
            WriteObjects(contents);
            WriteGlobals(contents);
        }

        private static void WriteGlobals(ZMemory contents)
        {
            WriteHeading("Globals");

            for (byte i = 0; i < ZGlobals.NumberOfGlobals; i++)
            {
                Console.WriteLine($"  [{i:D2}] = {contents.Globals.Get(i)}");
            }
        }

        private static void WriteObjects(ZMemory contents)
        {
            WriteHeading($"Object Tree ({contents.ObjectTree.Count} objects)");

            var sb = new StringBuilder();
            foreach (var zObj in contents.ObjectTree.Values)
            {
                sb.AppendLine(Format.Object(contents.ObjectTree, zObj.ObjectNumber, true));
            }

            Console.WriteLine(sb);
        }

        private static void WriteDictionary(ZMemory contents)
        {
            WriteHeading("Dictionary");

            var d = contents.Dictionary;

            var pairs = new Dictionary<string, string>
            {
                {$"InputCodes", $"{Format.ByteArray(d.InputCodes, false)} {Format.CharArray(d.InputCodes, false)}"}
            };
            Console.WriteLine(Format.KeyValues(pairs, 25));
            
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

        private static void WriteHeader(string filename, ZMemory contents)
        {
            Console.WriteLine($"File: {Path.GetFileNameWithoutExtension(filename)}");

            Console.WriteLine(Format.Header(contents.Header));
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
