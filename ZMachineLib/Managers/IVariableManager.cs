namespace ZMachineLib.Managers
{
    public interface IVariableManager
    {
        ushort GetUShort(byte variable, bool andRemove = true);
        void Store(byte dest, ushort value, bool newEntry = true);
        void Store(byte dest, byte value);
    }
}