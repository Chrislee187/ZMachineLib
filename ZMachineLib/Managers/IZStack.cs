using ZMachineLib.Content;

namespace ZMachineLib.Managers
{
    public interface IZStack : IZStack<ZStackFrame>
    {
        ushort PopCurrentRoutine();
        ushort PeekCurrentRoutine();
        bool CurrentRoutingAvailable();
        void PushNewRoutine(ushort routineAddress);

        ushort Variable(byte variable);
        void Variable(byte variable, ushort value);
        uint GetPC();
        void SetPC(uint value);
        uint GetPCAndInc();

        void IncrementPC(ushort value = 1); 
        void IncrementPC(uint value = 1);
        void IncrementPC(short value = 1); 
        void IncrementPC(int value = 1);

    }

    public interface IZStack<T>
    {
        T Peek();
        T Pop();
        void Push(T item);
        void Clear();
    }
}