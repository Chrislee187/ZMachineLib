using System;

namespace ZMachineLib.Managers
{
    public interface IMemoryManager
    {
        byte Get(uint address);
        byte Get(int address);
        byte Get(ushort address);
        ushort GetUShort(int address);
        uint GetUInt(int address);
        void Set(int address, byte value);
        void Set(ushort address, byte value);
        void Set(ushort address, ushort value);
        void SetWord(uint address, ushort value);
        void Set(ushort address, params byte[] values);
        void Set(int address, params byte[] values);
        Span<byte> AsSpan();
        Span<byte> AsSpan(ushort start);
        Span<byte> AsSpan(ushort start, int length);
        Span<byte> AsSpan(int start);
        Span<byte> AsSpan(uint start);
        void SetLong(uint address, uint value);
    }
}