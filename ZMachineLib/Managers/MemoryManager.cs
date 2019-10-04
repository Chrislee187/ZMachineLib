using System;
using ZMachineLib.Extensions;

namespace ZMachineLib.Managers
{
    public class MemoryManager : IMemoryManager
    {
        internal readonly byte[] Buffer;

        public MemoryManager(byte[] buffer)
        {
            Buffer = buffer;
        }
        
        public byte Get(uint address) => Buffer[address];
        public byte Get(int address) => Buffer[address];
        public byte Get(ushort address) => Buffer[address];
        public ushort GetUShort(int address) => Buffer.GetUShort(address);
        public uint GetUInt(int address) => Buffer.GetUInt(address);

        public byte[] AsSpan() => Buffer;
        public byte[] AsSpan(ushort start) => Buffer.AsSpan(start).ToArray();
        public byte[] AsSpan(int start) => Buffer.AsSpan(start).ToArray();
        public byte[] AsSpan(uint start) => Buffer.AsSpan((int)start).ToArray();
        public byte[] AsSpan(ushort start, int length) => Buffer.AsSpan(start, length).ToArray();

        public void Set(int address, byte value) => Buffer[address] = value;
        public void Set(ushort address, byte value) => Buffer[address] = value;

        public void Set(ushort address, ushort value)
        {
            Set(address, (byte)(value >> 8));
            Set(address + 1, (byte)(value >> 0));
        }

        public void Set(int address, params byte[] values)
            => Set((ushort) address, values);
        public void Set(ushort address, params byte[] values)
        {
            uint idx = 0;

            foreach (var value in values)
            {
                Set((ushort) (address + idx++),  value);
            }
        }

        public void SetUShort(uint address, ushort value)
        {
            Set((ushort) (address + 0), (byte) (value >> 8));
            Set((ushort) (address + 1), (byte) value);
        }
        public void SetUInt(uint address, uint value) => Buffer.SetLong(address, value);
    }
}