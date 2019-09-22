﻿using System;
using System.Runtime.InteropServices;
using ZMachineLib.Extensions;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace ZMachineLib.Content
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ZHeader
    {
        [FieldOffset(0x00)] public readonly byte Version;
        [FieldOffset(0x01)] [MarshalAs(UnmanagedType.U2)] private readonly ushort Flags1Raw;
        [FieldOffset(0x03)] private readonly byte Unknown1;
        [FieldOffset(0x04)] private readonly ushort HighMemoryBaseAddressRaw;
        [FieldOffset(0x06)] private readonly ushort ProgramCounterRaw;     // 0x06 (NB. Packed address of initial main routine in >= V6)
        [FieldOffset(0x08)] private readonly ushort DictionaryRaw;
        [FieldOffset(0x0a)] private readonly ushort ObjectTableRaw;
        [FieldOffset(0x0c)] private readonly ushort GlobalsRaw;
        [FieldOffset(0x0e)] private readonly ushort StaticMemoryBaseAddressRaw;
        [FieldOffset(0x10)] private readonly ushort Flags2Raw;
        [FieldOffset(0x12)] private readonly ushort Unknown2;
        [FieldOffset(0x14)] private readonly ushort Unknown3;
        [FieldOffset(0x16)] private readonly ushort Unknown4;
        [FieldOffset(0x18)] private readonly ushort AbbreviationsTableRaw;
        [FieldOffset(0x1a)] private readonly ushort LengthOfFileRaw;
        [FieldOffset(0x1c)] private readonly ushort ChecksumOfFileRaw;
        [FieldOffset(0x1e)] private readonly byte InterpreterNumber;
        [FieldOffset(0x1f)] private readonly byte InterpreterNumberVersion;


        public ushort Flags1 => Flags1Raw.SwapBytes();
        public ushort HighMemoryBaseAddress => HighMemoryBaseAddressRaw.SwapBytes();
        public ushort ProgramCounter => ProgramCounterRaw.SwapBytes();
        public ushort Dictionary => DictionaryRaw.SwapBytes();
        public ushort ObjectTable => ObjectTableRaw.SwapBytes();
        public ushort Globals => GlobalsRaw.SwapBytes();
        public ushort StaticMemoryBaseAddress => StaticMemoryBaseAddressRaw.SwapBytes();
        public ushort Flags2 => Flags2Raw.SwapBytes();
        public ushort AbbreviationsTable => AbbreviationsTableRaw.SwapBytes();
        public ushort LengthOfFile => LengthOfFileRaw.SwapBytes();
        public ushort ChecksumOfFile => ChecksumOfFileRaw.SwapBytes();

        public ushort Pc => ProgramCounter;
        public ushort DynamicMemorySize => StaticMemoryBaseAddress;

        public ZHeader(Span<byte> headerBytes)
        {
            GCHandle handle = GCHandle.Alloc(headerBytes.ToArray(), GCHandleType.Pinned);
            this = (ZHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(ZHeader));
            handle.Free();
        }
    }

    [Flags]
    // ReSharper disable UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public enum Flags1_V3 : byte
    {
        Bit0NotUsed = Bits.Bit0,
        StatusLineInTime = Bits.Bit1,
        TwoDiscStory = Bits.Bit2,
        Bit3NotUsed = Bits.Bit3,
        StatusLineNOTAvailable = Bits.Bit4,
        ScreenSplittingAvailable = Bits.Bit5,
        DefaultFontIsVariablePitch = Bits.Bit6,
        Bit7NotUsed = Bits.Bit7
    }

    public static class Flags1V3Extensions
    {
        public static bool HasFlagFast(this Flags1_V3 value, Flags1_V3 flag)
        {
            return (value & flag) != 0;
        }
    }
    // ReSharper restore UnusedMember.Global
}