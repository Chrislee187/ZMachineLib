using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
            var anyMoreEntries = true;

            var abbrIdx = 0;
            while (anyMoreEntries && abbrIdx < 96)
            {
                var addr = abbreviationsTable.GetUShort((ushort)(abbrIdx * 2));
                anyMoreEntries = addr != 0;
                if (anyMoreEntries)
                {
                    var zStr = ZsciiString.Get(dynamicMemory.AsSpan((ushort)(addr * 2)), null, header);
                    abbrevs.Add(zStr);
                    abbrIdx++;
                }
            }

//            for (int abbrIdx = 0; abbrIdx < 96 ; abbrIdx++)
//            {
//                var addr = abbreviationsTable.GetUShort((ushort) (abbrIdx * 2));
//
//                if (addr == 0) break;
//
//                var zStr = ZsciiString.Get(dynamicMemory.AsSpan((ushort)(addr * 2)), null);
//                abbrevs.Add(zStr);
//            }

            Abbreviations = abbrevs.ToArray();
        }
    }
}