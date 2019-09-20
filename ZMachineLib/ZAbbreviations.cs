using System;
using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib
{
    public class ZAbbreviations
    {
        public string[] Abbreviations { get; }
        public ZAbbreviations(Span<byte> abbreviationsTable, byte[] fullProgram)
        {
            abbreviationsTable.ToArray();
            var abbrevs = new List<string>();
            for (int abbrIdx = 0; abbrIdx < 96; abbrIdx++)
            {
                var addr = abbreviationsTable.GetUShort((ushort) (abbrIdx * 2));
                var zStr = ZsciiString.Get(fullProgram.AsSpan((ushort)(addr * 2)), null);
                abbrevs.Add(zStr);
            }

            Abbreviations = abbrevs.ToArray();
        }
    }
}