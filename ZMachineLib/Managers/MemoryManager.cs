using System;
using ZMachineLib.Extensions;

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

        void Set(ushort address, params byte[] values);
        void Set(int address, params byte[] values);
        Span<byte> AsSpan(ushort start);
        Span<byte> AsSpan(ushort start, int length);
        void SetLong(uint address, uint value);
    }

    public class MemoryManager : IMemoryManager
    {
        private readonly byte[] _memory;

        public MemoryManager(byte[] memory)
        {
            _memory = memory;
        }


        public byte Get(uint address) => _memory[address];
        public byte Get(int address) => _memory[address];
        public byte Get(ushort address) => _memory[address];
        public ushort GetUShort(int address) => _memory.GetUShort(address);
        public uint GetUInt(int address) => _memory.GetUInt(address);

        public Span<byte> AsSpan(ushort start) => _memory.AsSpan(start);
        public Span<byte> AsSpan(ushort start, int length) => _memory.AsSpan(start, length);

        public void Set(int address, byte value) => _memory[address] = value;
        public void Set(ushort address, byte value) => _memory[address] = value;

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
                _memory.Set(address + idx++, value);
            }
        }

        public void SetLong(uint address, uint value) => _memory.SetLong(address, value);
    }
}