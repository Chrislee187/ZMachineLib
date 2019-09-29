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
        void SetUShort(uint address, ushort value);
        void Set(ushort address, params byte[] values);
        void Set(int address, params byte[] values);
        byte[] AsSpan();
        byte[] AsSpan(ushort start);
        byte[] AsSpan(ushort start, int length);
        byte[] AsSpan(int start);
        byte[] AsSpan(uint start);
        void SetLong(uint address, uint value);
    }
}