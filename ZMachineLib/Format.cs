using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ZMachineLib.Content;

namespace ZMachineLib
{
    public static class Format
    {
        public static string KeyValues(IDictionary<string, string> pairs, int keyWidth = 0)
        {
            if (keyWidth == 0)
            {
                keyWidth = pairs.Keys.Max(k => k.Length) + 1;
            }

            var sb = new StringBuilder();
            foreach (var pair in pairs)
            {
                sb.AppendLine($"{pair.Key.PadRight(keyWidth)}:{pair.Value}");
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
            return $"{value:X2} ({ToBinary(value)})";
        }

        public static string Flags(ushort value)
        {
            var s = Convert
                .ToString(value, 2)
                .PadLeft(16, '0');
            return $"{value:X4} ({ToBinary(value)})";
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
                var test = (flag & attributes) == flag;
                if (test)
                {
                    attrs.Add(i);
                }

                binary += test ? "1" : "0";
            }

            return $"{string.Join(",", attrs.Select(a => a.ToString()))} ({attributes.ToBinary()})";
        }

        public static string ToBinary(this ulong value)
        {
            var binary = "";
            for (int i = 0; i < 32; i++)
            {
                ulong flag = (ulong) 1 << i;

                binary += (flag & value) == flag ? "1" : "0";
            }

            return $"0b{binary}";
        }
        public static string ToBinary(this ushort value)
        {
            var binary = "";
            for (int i = 0; i < 16; i++)
            {
                ulong flag = (ulong)1 << i;

                binary += (flag & value) == flag ? "1" : "0";
            }

            return $"0b{binary}";
        }

        public static string ToBinary(this byte value)
        {
            var binary = "";
            for (int i = 0; i < 8; i++)
            {
                ulong flag = (ulong)1 << i;

                binary += (flag & value) == flag ? "1" : "0";
            }

            return $"0b{binary}";
        }

        public static string Header(ZHeader header, int col1Width = 25)
        {
            var h = header;

            var pairs = new Dictionary<string, string>
            {
                {"ZMachine Version", $"{h.Version}"},
                {"Flags1", Flags((byte) h.Flags1)},
                {"Flags2", Flags(h.Flags2)},
                {"Abbreviations Table", Word(h.AbbreviationsTable)},
                {"Object Table", Word(h.ObjectTable)},
                {"Globals", Word(h.Globals)},
                {"Dynamic Memory Size", Word(h.DynamicMemorySize)},
                {"Static Memory", Word(h.StaticMemoryBaseAddress)},
                {"Dictionary", Word(h.Dictionary)},
                {"High Memory", Word(h.HighMemoryBaseAddress)},
                {"Program Counter:", Word(h.ProgramCounter)},
                {"Unknown1 (0x01):", Byte(h.Unknown1)},
                {"Unknown2 (0x12):", Word(h.Unknown2)},
                {"Unknown3 (0x14):", Word(h.Unknown3)},
                {"Unknown4 (0x16):", Word(h.Unknown4)}
            };
            
            return KeyValues(pairs);
        }

        public static string Object(IZObjectTree objs, ushort objNumber, bool showAttrs = false)
        {
            var zObj = objs.GetOrDefault(objNumber);
            var sb = new StringBuilder();
            sb.Append($"{zObj}");

            if (showAttrs)
            {
                sb.Append($" Value: {Attributes(zObj.Attributes.Value)}");
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