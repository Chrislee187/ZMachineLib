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

        protected byte[] Memory => Machine.Memory;
        protected ushort GlobalsTable => Machine.Contents.Header.Globals;
        protected ushort ObjectTable => Machine.Contents.Header.ObjectTable;
        protected VersionedOffsets Offsets => Machine.Contents.Offsets;
        protected Stack<ZStackFrame> Stack => Machine.Stack;
        protected byte Version => Machine.Contents.Header.Version;
    }
}