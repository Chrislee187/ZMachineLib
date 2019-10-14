using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using ZMachineLib.Extensions;
using ZMachineLib.Operations.OP2;

namespace ZMachineLib.Content
{
    public partial class ZsciiString
    {
        private ZHeader _header;
        public static string Get(Span<byte> data, ZAbbreviations abbreviations, ZHeader header) => new ZsciiString(data, abbreviations, header).String;

        public string String { get; }

        public ushort BytesUsed { get; }
        private readonly V2CharProvider _v2CharProvider;

        public ZsciiString(Span<byte> data, ZAbbreviations abbreviations, ZHeader header,
            int zsciiWordsToUse = int.MaxValue)
        {
            _v2CharProvider = new V2CharProvider(abbreviations);
            _header = header;
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
        {
            return c >= 0x01 && c <= 0x03;
//            return _header.Version < 3 
//                ? c == 1 
//                : c >= 0x01 && c <= 0x03;
        }


        private string DecodeZsciiChars(List<byte> zChars, ZAbbreviations abbreviations)
        {
            _v2CharProvider.ResetTable();

            var sb = new StringBuilder();

            for (var i = 0; i < zChars.Count; i++)
            {
                if (zChars[i] == 0x00)
                    sb.Append(" ");
                else if (_header.Version <= 2)
                {
                    var zChar = zChars[i];

                    if (zChar == 1)
                    {
                        var abbrIdx = zChars[++i];
                        sb.Append(abbreviations.Abbreviations[abbrIdx]);
                    }
                    else
                    {
                        var (zCharTable, inc) = _v2CharProvider.GetCharTable(zChar);
                        i += inc;

                        if (i < zChars.Count)
                        {
                            zChar = zChars[i];
                            if (zChar == 6 && zCharTable[0] == ' ') 
                            {
                                ushort x = (ushort)(zChars[i + 1] << 5 | zChars[i + 2]);
                                sb.Append(Convert.ToChar(x));
                                i += 2;
                            }

                            if (zChar > 5)
                            {
                                var realChar = zCharTable[zChar - 6];
                                sb.Append(realChar);
                            }
                        }
                    }
                }
//                else if (zChars[i] >= 0x01 && zChars[i] <= 0x03)
//                {
//                    ushort offset = (ushort)(32 * (chars[i] - 1) + chars[++i]);
//                    ushort lookup = (ushort)(_abbreviationsTable + (offset * 2));
//                    ushort wordAddr = GetWord(lookup);
//                    List<byte> abbrev = GetZsciiChars((ushort)(wordAddr * 2));
//                    sb.Append(DecodeZsciiChars(abbrev));
//                }
                //                else if (zChars[i] == 0x02 && _header.Version <= 2)
                //                {
                //                    sb.Append(Convert.ToChar((zChars[++i] - 6) + 'A'));
                //                }
                //                else if (zChars[i] == 0x03 && _header.Version <= 2)
                //                {
                //                    if (zChars.Count < i + 1)
                //                    {
                //                        sb.Append(Table[zChars[++i] - 6]);
                //                    }
                //                }
                //                else if (zChars[i] == 0x04 && _header.Version <= 2)
                //                {
                //                    sb.Append(Convert.ToChar((zChars[++i] - 6) + 'A'));
                //                }
                //                else if (zChars[i] == 0x05 && _header.Version <= 2)
                //                {
                //                    if (zChars.Count < i + 1)
                //                    {
                //                        sb.Append(Table[zChars[++i] - 6]);
                //                    }
                //                }
                else if (abbreviations != null && IsAbbreviation(zChars[i]))
                {
                    if (i + 1 <= zChars.Count - 1)
                    {
                        var offset = (ushort)(32 * (zChars[i] - 1) + zChars[++i]);
                        sb.Append(abbreviations.Abbreviations[offset]);
                    }
                }

                else if (zChars[i] == 0x04)
                    sb.Append(Convert.ToChar((zChars[++i] - 6) + 'A'));
                else if (zChars[i] == 0x05)
                {
                    if (i == zChars.Count - 1 || zChars[i + 1] == 0x05)
                        break;

                    if (zChars[i + 1] == 0x06)
                    {
                        var x = (ushort)(zChars[i + 2] << 5 | zChars[i + 3]);
                        i += 3;
                        sb.Append(Convert.ToChar(x));
                    }
                    else if (zChars[i + 1] == 0x07)
                    {
                        sb.AppendLine("");
                        i++;
                    }
                    else
                    {
                        sb.Append(Table[zChars[++i] - 6]);
                    }

                }
                else
                    sb.Append(Convert.ToChar((zChars[i] - 6) + 'a'));
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