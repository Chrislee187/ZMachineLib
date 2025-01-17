﻿using System;
using ZMachineLib.Extensions;

namespace ZMachineLib.Content
{
    /// <summary>
    /// Expects the bytes of data to represent a collection of Zscii strings where
    /// Byte 0: Length of each entry
    /// Byte 1-2: (ushort) Number of entries
    /// Byte 3.. : individual zscii strings
    /// </summary>
    public class ZsciiStringArray
    {
        public byte EntryLength { get; }
        public ushort WordStart { get; }

        public string[] Words { get; }
        public ZsciiStringArray(Span<byte> data, ZAbbreviations abbreviations, ZHeader header)
        {
            var bytes = data.ToArray();

            ushort ptr = 0;
            EntryLength = bytes[ptr++];
            var numEntries = bytes.GetUShort(ptr);
            ptr += 2;

            WordStart = ptr;

            Words = new string[numEntries];
            for (var i = 0; i < numEntries; i++)
            {
                var stringData = bytes.AsSpan(ptr);
                var zStr = new ZsciiString(stringData,abbreviations, header, header.Version <= 3 
                    ? 2 : 3);

                ptr += EntryLength;
                Words[i] = zStr.String;
            }
        }
    }
}