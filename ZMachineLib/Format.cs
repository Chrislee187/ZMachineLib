using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ZMachineLib.Content;

namespace ZMachineLib
{
    public static class Format
    {
        public static string TwoColumn(IDictionary<string, string> pairs, int col1Width)
        {
            var sb = new StringBuilder();
            foreach (var pair in pairs)
            {
                sb.AppendLine($"{pair.Key.PadRight(col1Width)}:{pair.Value}");
            }

            return sb.ToString();
        }

        public static string Word(ushort value) => $"{value:X4} ({value})";
        public static string Byte(byte value) => $"{value:X2} ({value})";

        public static string Flags(byte value)
        {
            var s = Convert
                .ToString(value, 2)
                .PadLeft(8, '0');
            return $"{value:X2} ({s})";
        }

        public static string Flags(ushort value)
        {
            var s = Convert
                .ToString(value, 2)
                .PadLeft(16, '0');
            return $"{value:X4} ({s})";
        }

        public static string ByteArray(Span<byte> bytes, bool newline = true)
        {
            var sb = new StringBuilder();

            foreach (var b in bytes)
            {
                sb.Append($"{b:X2} ");
            }

            if (newline)
            {
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string CharArray(byte[] bytes, bool newline = true)
        {
            var sb = new StringBuilder();

            foreach (var b in bytes)
            {
                sb.Append($"'{(char)b}' ");
            }

            if (newline)
            {
                sb.AppendLine();
            }
            
            return sb.ToString();
        }

        public static string Attributes(ulong attributes)
        {
            var attrs = new List<int>();
            var binary = "";
            for (int i = 0; i < 32; i++)
            {
                ulong flag = 0x80000000 >> i;
                if ((flag & attributes) == flag)
                {
                    attrs.Add(i);
                }

                binary += (flag & attributes) == flag ? "1" : "0";
            }

            return $"{string.Join(',', attrs)} (0b{binary.Reverse()})";
        }

        public static string Header(ZHeader header, int col1Width = 25)
        {
            var h = header;

            var pairs = new Dictionary<string, string>();

            pairs.Add($"ZMachine Version", $"{h.Version}");
            pairs.Add("Addresses", "");
            pairs.Add($"Program Counter:", Word(h.ProgramCounter));
            pairs.Add($"High Memory", Word(h.HighMemoryBaseAddress));
            pairs.Add($"Static Memory", Word(h.StaticMemoryBaseAddress));
            pairs.Add($"Dictionary", Word(h.Dictionary));
            pairs.Add($"Object Table", Word(h.ObjectTable));
            pairs.Add($"Globals", Word(h.Globals));
            pairs.Add($"Abbreviations Table", Word(h.AbbreviationsTable));
            pairs.Add($"Dynamic Memory Size", Word(h.DynamicMemorySize));
            pairs.Add($"Flags1", Flags((byte)h.Flags1));
            pairs.Add("Flags2", Flags(h.Flags2));

            return TwoColumn(pairs, col1Width);
        }

        public static string Object(IZObjectTree objs, ushort objNumber, bool showAttrs = false)
        {
            var zObj = objs.GetOrDefault(objNumber);
            var sb = new StringBuilder();
            sb.Append($"{zObj}");

            if (showAttrs)
            {
                sb.Append($" Attributes: {Attributes(zObj.Attributes)}");
            }

            sb.AppendLine();

            if (zObj.Parent != 0)
            {
                sb.AppendLine($"   Parent: {objs[zObj.Parent]}");
            }

            if (zObj.Sibling != 0)
            {
                sb.AppendLine($"  Sibling: {objs[zObj.Sibling]}");
            }

            if (zObj.Child != 0)
            {
                sb.AppendLine($"    Child: {objs[zObj.Child]}");
            }

            sb.AppendLine($"  Properties:");
            foreach (var pair in zObj.Properties.OrderBy(p => p.Key))
            {
                sb.AppendLine($"   [{pair.Key:D2}] : {ByteArray(pair.Value.Data, false)}");

            }

            return sb.ToString();
        }

        public static string Reverse(this string text)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(text);

            var elements = new List<string>();
            while (enumerator.MoveNext())
                elements.Add(enumerator.GetTextElement());

            elements.Reverse();
            return string.Concat(elements);
        }

        public static string Globals(IZGlobals globals)
        {
            var sb = new StringBuilder();
            for (byte i = 0; i < ZGlobals.NumberOfGlobals; i++)
            {
                sb.AppendLine($"  {Global(globals, i)}");
            }

            return sb.ToString();
        }

        public static string Global(IZGlobals globals, byte globalNumb) 
            => $"  #{Byte(globalNumb)} = {Word(globals.Get(globalNumb))}";
    }
}