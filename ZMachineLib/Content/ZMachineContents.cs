using System;

namespace ZMachineLib.Content
{
    public class ZMachineContents
    {
        public byte Version { get; }
        public ZHeader Header { get; }
        public ZDictionary Dictionary { get; }
        public ZAbbreviations Abbreviations { get; }
        public ZObjectTree ObjectTree { get; }

        public byte[] DynamicMemory { get; }

        public ZMachineContents(byte[] data)
        {
            Version = data[0];

            if (Version > 3) throw new NotSupportedException("ZMachine > V3 not currently supported");

            Header = new ZHeader(data.AsSpan(0,31));
            DynamicMemory = data.AsSpan(0, Header.DynamicMemorySize).ToArray();
            Abbreviations = new ZAbbreviations(data.AsSpan(Header.AbbreviationsTable), DynamicMemory);
            Dictionary = new ZDictionary(data.AsSpan(Header.Dictionary), Abbreviations);
            ObjectTree = new ZObjectTree(DynamicMemory, Header, Abbreviations);
        }
    }
}