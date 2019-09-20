using System;
using System.Linq;

namespace ZMachineLib
{
    public class ZMachineContents
    {
        public byte Version { get; }
        public Header Header { get; }
        public ZDictionary Dictionary { get; }
        public ZAbbreviations Abbreviations { get; }

        public ZMachineContents(byte[] data)
        {
            Version = data[0];

            if (Version > 3) throw new NotSupportedException("ZMachine > V3 not currently supported");

            Header = new Header(data.AsSpan(0,31));
            Abbreviations = new ZAbbreviations(data.AsSpan(Header.AbbreviationsTable), data.ToArray());
            Dictionary = new ZDictionary(data.AsSpan(Header.Dictionary), Abbreviations);
        }


    }
}