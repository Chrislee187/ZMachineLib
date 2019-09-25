using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Managers;

namespace ZMachineLib
{
    public abstract class ZMachineBase
    {
        protected IMemoryManager MemoryManager;
        public ZMachine2 Machine { get; }

        protected ZMachineBase(ZMachine2 machine, 
            IMemoryManager memoryManager)
        {
            MemoryManager = memoryManager;
            Machine = machine;
        }
    }
}