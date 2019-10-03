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
        public uint GetPCAndInc()
        {
            var pc = GetPC();
            IncrementPC(1);

            return pc;
        }

        public uint GetPC() => Peek().PC;
        public void SetPC(uint value) => Peek().PC = value;
//        public void IncrementPC(ushort value = 1) => Peek().PC += value;
//        public void IncrementPC(uint value = 1) => Peek().PC += value;
//        public void IncrementPC(short value = 1) => Peek().PC += (uint) value;
        public void IncrementPC(int value = 1) => Peek().PC += (uint) value;
    }

}