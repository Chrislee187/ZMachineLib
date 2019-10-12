using System;
using System.Collections.Generic;
using System.Text;
using ZMachineLib.Extensions;

namespace ZMachineLib.Content
{
    public class ZsciiString
    {
        public static string Get(Span<byte> data, ZAbbreviations abbreviations) => new ZsciiString(data, abbreviations).String;

        public string String { get; }

        public ushort BytesUsed { get; }

        public ZsciiString(Span<byte> data, ZAbbreviations abbreviations)
        {
            var chars = GetZsciiChars(data);

            BytesUsed = (ushort)(chars.Count / 3 * 2);

            String = DecodeZsciiChars(chars, abbreviations);
        }
        public ZsciiString(Span<byte> data, int zsciiWordsToUse, ZAbbreviations abbreviations)
        {
            var chars = GetZsciiChars(data, zsciiWordsToUse);

            BytesUsed = (ushort)(chars.Count / 3 * 2);

            String = DecodeZsciiChars(chars, abbreviations);
        }
        private List<byte> GetZsciiChars(Span<byte> data, int zsciiWordsToUse = int.MinValue)
        {
            var chars = new List<byte>();
            var ptr = 0;
            ushort word;
            do
            {
                word = data.Slice(ptr, 2).GetUShort();
                chars.AddRange(GetZCharsFromWord(word));
                ptr += 2;
            }
            while ((word & LastZChars) != LastZChars 
                   && (ptr / 2 ) != zsciiWordsToUse );

            return chars;
        }

        private bool IsAbbreviation(byte c)
            => c >= 0x01 && c <= 0x03;

        private string DecodeZsciiChars(List<byte> chars, ZAbbreviations abbreviations)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < chars.Count; i++)
            {
                if (chars[i] == 0x00)
                    sb.Append(" ");
                else if (abbreviations != null && IsAbbreviation(chars[i]))
                {
                    var offset = (ushort)(32 * (chars[i] - 1) + chars[++i]);
                    sb.Append(abbreviations.Abbreviations[offset]);
                }
                else if (chars[i] == 0x04)
                    sb.Append(Convert.ToChar((chars[++i] - 6) + 'A'));
                else if (chars[i] == 0x05)
                {
                    if (i == chars.Count - 1 || chars[i + 1] == 0x05)
                        break;

                    if (chars[i + 1] == 0x06)
                    {
                        var x = (ushort)(chars[i + 2] << 5 | chars[i + 3]);
                        i += 3;
                        sb.Append(Convert.ToChar(x));
                    }
                    else if (chars[i + 1] == 0x07)
                    {
                        sb.AppendLine("");
                        i++;
                    }
                    else
                    {
                        sb.Append(Table[chars[++i] - 6]);
                    }

                }
                else
                    sb.Append(Convert.ToChar((chars[i] - 6) + 'a'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// <see cref="http://inform-fiction.org/zmachine/standards/z1point1/sect03.html"/> S3.2
        /// </summary>
        private const string Table = @" ^0123456789.,!?_#'""/\-:()";
        private const ushort LastZChars = 0x8000;

        /// <summary>
        /// Create a mask to extract the three individual zChars stored in two bytes
        /// </summary>
        /// <param name="word"></param>
        /// <param name="charIdx"></param>
        /// <returns></returns>
        private byte CharMask(ushort word, ZCharShift charIdx) 
            => (byte)(word >> (ushort)charIdx & 0x1f);

        private enum ZCharShift : ushort
        {
            Char1 = 10, Char2 = 5, Char3 = 0
        }

        private IEnumerable<byte> GetZCharsFromWord(ushort word)
        {
            var chars = new List<byte>
            {
                CharMask(word, ZCharShift.Char1),
                CharMask(word, ZCharShift.Char2),
                CharMask(word, ZCharShift.Char3)
            };
            return chars;
        }
    }
}