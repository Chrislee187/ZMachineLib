using System;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public class ZDictionary
    {
        public byte[] InputCodes { get; }
        public ushort WordStart { get; }

        public string[] Words { get; }
        public ZDictionary(ZHeader header, IMemoryManager manager, ZAbbreviations abbreviations)
        {
            var bytes = manager.AsSpan(header.Dictionary);
            ushort addr = 0;

            var numOfInputCodes = bytes[addr++];
            InputCodes = bytes.AsSpan(addr, numOfInputCodes - 1).ToArray();
            addr += numOfInputCodes;

            var zsciiStringArray = new ZsciiStringArray(bytes.AsSpan(addr), abbreviations);
            Words = zsciiStringArray.Words;
            EntryLength = zsciiStringArray.EntryLength;
            WordStart = (ushort) (addr + zsciiStringArray.WordStart);

        }

        public byte EntryLength { get; set; }
    }
}