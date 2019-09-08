using System;
using System.Collections.Generic;
using System.Text;

namespace ZMachineLib.Operations.Kind0
{
    public abstract class BasePrintingOperations : ZMachineOperation
    {
        protected readonly IZMachineIO Io;
        protected BasePrintingOperations(Kind0OpCodes code,
            ZMachine2 machine,
            IZMachineIO io) 
            : base(code, machine)
        {
            Io = io;
        }

        protected readonly string Table = @" ^0123456789.,!?_#'""/\-:()";

        protected string GetZsciiString()
        {
            var chars = GetZsciiChars(Machine.Stack.Peek().PC);
            Machine.Stack.Peek().PC += (ushort)(chars.Count / 3 * 2);
            return DecodeZsciiChars(chars);
        }

        private string DecodeZsciiChars(List<byte> chars)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < chars.Count; i++)
            {
                if (chars[i] == 0x00)
                    sb.Append(" ");
                else if (chars[i] >= 0x01 && chars[i] <= 0x03)
                {
                    var offset = (ushort)(32 * (chars[i] - 1) + chars[++i]);
                    var lookup = (ushort)(Machine.AbbreviationsTable + (offset * 2));
                    var wordAddr = GetWord(lookup);
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
    }
}