using System;
using System.Collections.Generic;
using System.Text;

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
    }
}