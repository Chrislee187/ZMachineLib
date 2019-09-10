using System;
using System.Collections.Generic;
using System.Text;

namespace ZMachineLib.Operations.Kind0
{
    public class ZsciiString
    {
        public ZMachine2 Machine { get; }

        protected readonly string Table = @" ^0123456789.,!?_#'""/\-:()";

        public ZsciiString(ZMachine2 machine)
        {
            Machine = machine;
        }

        public string GetZsciiString()
        {
            var chars = GetZsciiChars(Machine.Stack.Peek().PC);
            Machine.Stack.Peek().PC += (ushort)(chars.Count / 3 * 2);
            return DecodeZsciiChars(chars);
        }
        public string GetZsciiString(uint address)
        {
            var chars = GetZsciiChars(address);
            return DecodeZsciiChars(chars);
        }
        public string DecodeZsciiChars(List<byte> chars)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < chars.Count; i++)
            {
                if (chars[i] == 0x00)
                    sb.Append(" ");
                else if (chars[i] >= 0x01 && chars[i] <= 0x03)
                {
                    var offset = (ushort)(32 * (chars[i] - 1) + chars[++i]);
                    var lookup = (ushort)(Machine.Header.AbbreviationsTable + (offset * 2));
                    var wordAddr = Machine.GetWord(lookup);
                    var abbrev = GetZsciiChars((ushort)(wordAddr * 2));
                    sb.Append(DecodeZsciiChars(abbrev));
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
                        sb.Append(Table[chars[++i] - 6]);
                }
                else
                    sb.Append(Convert.ToChar((chars[i] - 6) + 'a'));
            }
            return sb.ToString();
        }


        public List<byte> GetZsciiChars(uint address)
        {
            var chars = new List<byte>();
            ushort word;
            do
            {
                word = Machine.GetWord(address);
                chars.AddRange(GetZsciiChar(address));
                address += 2;
            }
            while ((word & 0x8000) != 0x8000);

            return chars;
        }

        public List<byte> GetZsciiChar(uint address)
        {
            var chars = new List<byte>();

            var word = Machine.GetWord(address);

            var c = (byte)(word >> 10 & 0x1f);
            chars.Add(c);
            c = (byte)(word >> 5 & 0x1f);
            chars.Add(c);
            c = (byte)(word >> 0 & 0x1f);
            chars.Add(c);

            return chars;
        }
    }
}