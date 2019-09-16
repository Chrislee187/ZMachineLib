using System.Runtime.InteropServices;
using ZMachineLib.Extensions;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace ZMachineLib
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Header
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

        public  Header(byte[] headerBytes)
        {
            GCHandle handle = GCHandle.Alloc(headerBytes, GCHandleType.Pinned);
            this = (Header)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Header));
            handle.Free();
        }
    }
}