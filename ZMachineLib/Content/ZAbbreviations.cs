using System;
using System.Collections.Generic;
using ZMachineLib.Extensions;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public class ZAbbreviations
    {
        public string[] Abbreviations { get; }
        public ZAbbreviations(ZHeader header, IMemoryManager manager)
        {
            var abbreviationsTable = manager.AsSpan(header.AbbreviationsTable).ToArray();
            var dynamicMemory = manager.AsSpan(0, header.DynamicMemorySize);
            var abbrevs = new List<string>();
            for (int abbrIdx = 0; abbrIdx < 96; abbrIdx++)
            {
                var addr = abbreviationsTable.GetUShort((ushort) (abbrIdx * 2));
                var zStr = ZsciiString.Get(dynamicMemory.Slice((ushort)(addr * 2)), null);
                abbrevs.Add(zStr);
            }

            Abbreviations = abbrevs.ToArray();
        }
    }
}