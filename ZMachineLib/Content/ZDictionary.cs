using System;

namespace ZMachineLib.Content
{
    public class ZDictionary
    {
        public byte[] InputCodes { get; }
        public ushort WordStart { get; }

        public string[] Words { get; }
        public ZDictionary(Span<byte> data, ZAbbreviations abbreviations)
        {
            var bytes = data.ToArray();
            ushort ptr = 0;

            var numOfInputCodes = bytes[ptr++];
            InputCodes = bytes.AsSpan(ptr, numOfInputCodes - 1).ToArray();
            ptr += numOfInputCodes;

            var zsciiStringArray = new ZsciiStringArray(bytes.AsSpan(ptr), abbreviations);
            Words = zsciiStringArray.Words;
            EntryLength = zsciiStringArray.EntryLength;
            WordStart = (ushort) (ptr + zsciiStringArray.WordStart);

        }

        public byte EntryLength { get; set; }
    }
}