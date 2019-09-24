namespace ZMachineLib.Content
{
    public class ZProperty
    {
        public byte[] Data { get; }
        public byte Number { get; }
        public ushort DataAddress { get; }

        public ZProperty(byte number, ushort dataAddress, byte[] data)
        {
            Data = data;
            Number = number;
            DataAddress = dataAddress;
        }
    }
}