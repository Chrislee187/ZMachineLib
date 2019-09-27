namespace ZMachineLib.Content
{
    public interface IZGlobals
    {
        ushort Get(byte globalNumber);
        void Set(byte globalNumber, ushort value);
    }
}