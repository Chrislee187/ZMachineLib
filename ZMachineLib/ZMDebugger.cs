using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMachineLib.Content;

namespace ZMachineLib
{
    public class ZMDebugger
    {
        private string PROMPT = ">";
        public (bool isDebug, string output) HandleDebugCommands(IZMemory memory, string commandLine)
        {
            var sb = new StringBuilder();
            // TODO: Pull this out in to a seperate class that returns the debug output strings to be displayed
            var args = commandLine.Split(' ');
            var cmd = args.First();
            var debugging = cmd.StartsWith("!!");
            if (debugging)
            {
                switch (cmd.ToLower())
                {
                    case "!!header":
                        sb.Append(DebugHeader(memory.Header));
                        break;
                    case "!!globals":
                        sb.Append(DebugGlobals(memory));
                        break;
                    case "!!global":
                        sb.Append(DebugGlobal(memory, byte.Parse(args[1])));
                        break;
                    case "!!object":
                        sb.Append(DebugObject(memory, byte.Parse(args[1])));
                        break;
                    default:
                        sb.AppendLine($"Unknown debug command: {commandLine}\nValid commands: " +
                                      $"header, globals, global #, object #");
                        break;
                }

                sb.Append(PROMPT);
            }

            return (debugging, sb.ToString());
        }

        private static string DebugObject(IZMemory memory, byte objectNumber)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[{Format.Byte(objectNumber)}] = {memory.ObjectTree.GetOrDefault(objectNumber).Name}");
            return sb.ToString();
        }

        private static string DebugGlobal(IZMemory memory, byte globalNumber)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Global #{Format.Byte(globalNumber)} = {Format.Word(memory.Globals.Get(globalNumber))}");
            return sb.ToString();
        }
        private static string DebugGlobals(IZMemory memory)
        {
            var sb = new StringBuilder();
            for (byte i = 0; i < ZGlobals.NumberOfGlobals; i++)
            {
                sb.AppendLine($"  #{Format.Byte(i)} = {Format.Word(memory.Globals.Get(i))}");
            }

            return sb.ToString();
        }

        private static string DebugHeader(ZHeader memoryHeader)
        {
            var h = memoryHeader;

            var pairs = new Dictionary<string, string>();

            pairs.Add($"ZMachine Version", $"{h.Version}");
            pairs.Add("Addresses", "");
            pairs.Add($"Program Counter:", Format.Word(h.ProgramCounter));
            pairs.Add($"High Memory", Format.Word(h.HighMemoryBaseAddress));
            pairs.Add($"Static Memory", Format.Word(h.StaticMemoryBaseAddress));
            pairs.Add($"Dictionary", Format.Word(h.Dictionary));
            pairs.Add($"Object Table", Format.Word(h.ObjectTable));
            pairs.Add($"Globals", Format.Word(h.Globals));
            pairs.Add($"Abbreviations Table", Format.Word(h.AbbreviationsTable));
            pairs.Add($"Dynamic Memory Size", Format.Word(h.DynamicMemorySize));
            pairs.Add($"Flags1", Format.Flags((byte)h.Flags1));
            pairs.Add("Flags2", Format.Flags(h.Flags2));

            return Format.TwoColumn(pairs, 25);
        }
    }
}