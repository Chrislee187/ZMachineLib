namespace ZMachineLib
{
    public interface IMemoryManager
    {
        byte Get(uint address);
        byte Get(int address);
        byte Get(ushort address);
        void Set(int address, byte value);
        void Set(ushort address, byte value);
        void Set(ushort address, ushort value);
    }

    public class MemoryManager : IMemoryManager
    {
        protected ZMachine2 Machine;

        public MemoryManager(ZMachine2 machine)
        {
            Machine = machine;
        }


        public byte Get(uint address) => Machine.Memory[address];
        public byte Get(int address) => Machine.Memory[address];
        public byte Get(ushort address) => Machine.Memory[address];

        public void Set(int address, byte value) => Machine.Memory[address] = value;
        public void Set(ushort address, byte value) => Machine.Memory[address] = value;
        public void Set(ushort address, ushort value)
        {
            Machine.Memory[address] = (byte)(value >> 8);
            Machine.Memory[address+1] = (byte)(value >> 0);
        }
    }
}