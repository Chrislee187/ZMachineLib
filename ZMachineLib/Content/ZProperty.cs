using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public class ZProperty
    {
        private byte[] _data;
        private readonly IMemoryManager _manager;

        public byte[] Data
        {
            get => _data;
            set
            {
                _data = value;
                _manager.Set(DataAddress, value);
            }

        }

        public byte Number { get; }
        public ushort DataAddress { get; }

        public ZProperty(byte number, ushort dataAddress, byte[] data, 
            IMemoryManager manager)
        {
            _manager = manager;
            Data = data;
            Number = number;
            DataAddress = dataAddress;
        }
    }
}