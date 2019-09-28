using System.Collections.Generic;
using System.Linq;
using ZMachineLib.Content;

namespace ZMachineLib.Managers
{
    public class ZStack : Stack<ZStackFrame>, IZStack
    {
        public ushort PopCurrentRoutine() => Peek().RoutineStack.Pop(); 

        public ushort PeekCurrentRoutine()=> Peek().RoutineStack.Peek();

        public bool CurrentRoutingAvailable() => Peek().RoutineStack.Any();

        public void PushNewRoutine(ushort value) => Peek().RoutineStack.Push(value);


        public ushort Variable(byte variable) => Peek().Variables[variable];
        public void Variable(byte variable, ushort value) => Peek().Variables[variable] = value;
        public uint GetPCAndInc() => Peek().PC++;
        public uint GetPC() => Peek().PC;
        public void SetPC(uint value) => Peek().PC = value;
        public void IncrementPC(ushort value = 1) => Peek().PC += value;
        public void IncrementPC(uint value = 1) => Peek().PC += value;
        public void IncrementPC(short value = 1) => Peek().PC += (uint) value;
        public void IncrementPC(int value = 1) => Peek().PC += (uint) value;

        Stack<ZStackFrame> RootStack()
        {
            return this;
        }
    }

    public interface IStack<T>
    {
        T Peek();
        T Pop();
        void Push(T item);
        void Clear();
    }
    public interface IZStack : IStack<ZStackFrame>
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
}