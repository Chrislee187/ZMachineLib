using ZMachineLib.Extensions;

namespace ZMachineLib.Managers
{
    public interface IMemoryManager
    {
        byte Get(uint address);
        byte Get(int address);
        byte Get(ushort address);
        ushort GetUShort(int address);
        void Set(int address, byte value);
        void Set(ushort address, byte value);
        void Set(ushort address, ushort value);
    }

    public class MemoryManager : IMemoryManager
    {
        private readonly ZMachine2 _machine;

        public MemoryManager(ZMachine2 machine)
        {
            _machine = machine;
        }


        public byte Get(uint address) => _machine.Memory[address];
        public byte Get(int address) => _machine.Memory[address];
        public byte Get(ushort address) => _machine.Memory[address];
        public ushort GetUShort(int address) => _machine.Memory.GetUShort(address);

        public void Set(int address, byte value) => _machine.Memory[address] = value;
        public void Set(ushort address, byte value) => _machine.Memory[address] = value;
        public void Set(ushort address, ushort value)
        {
            _machine.Memory[address] = (byte)(value >> 8);
            _machine.Memory[address+1] = (byte)(value >> 0);
        }
    }
}