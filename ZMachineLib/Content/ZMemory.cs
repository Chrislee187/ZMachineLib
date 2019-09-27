using System;
using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public interface IZMemory
    {
        Stack<ZStackFrame> Stack { get; set; }
        ZHeader Header { get; }
        ZDictionary Dictionary { get; }
        ZAbbreviations Abbreviations { get; }
        IZObjectTree ObjectTree { get; }
        IMemoryManager Manager { get; }
        IVariableManager VariableManager { get; }
        OperandManager OperandManager { get; }
        VersionedOffsets Offsets { get; }
        ushort DictionaryWordStart { get; }
        byte PeekNextByte();
        byte PeekPreviousByte();
        byte PeekCurrentByte();
        byte GetCurrentByteAndInc();
        uint GetPackedAddress(ushort address);

        bool TerminateOnInput { get; set; }
        ushort ReadTextAddr { get; set; }
        ushort ReadParseAddr { get; set; }

        bool Running { get; set; }
        ZGlobals Globals { get; set; }

        void Restart();
    }

    public class ZMemory : IZMemory
    {
        private readonly Action _restart;
        public Stack<ZStackFrame> Stack { get; set; }
        public ZHeader Header { get; }
        public ZDictionary Dictionary { get; }
        public ZAbbreviations Abbreviations { get; }
        public IZObjectTree ObjectTree { get; }
        public IMemoryManager Manager { get; }
        public IVariableManager VariableManager { get; }
        public OperandManager OperandManager { get; }
        public ZGlobals Globals { get; set; }


        public byte[] Memory { get; }
        public VersionedOffsets Offsets { get; private set; }
        public ushort DictionaryWordStart => (ushort) (Header.Dictionary + Dictionary.WordStart);

        public ZMemory(byte[] data,
            Action restart)
        {
            _restart = restart;
            Memory = data;
            Header = new ZHeader(data.AsSpan(0, 31));
            if (Header.Version > 3) throw new NotSupportedException("ZMachine > V3 not currently supported");

            // Version specific offsets - not used every yet
            Offsets = VersionedOffsets.For(Header.Version);

            Manager = new MemoryManager(Memory);
            
            // ZMachine tables
            Abbreviations = new ZAbbreviations(Header, Manager);
            Dictionary = new ZDictionary(Header, Manager, Abbreviations);
            ObjectTree = new ZObjectTree(Header, Manager, Abbreviations);
            Globals = new ZGlobals(Header, Manager);

            Stack = new Stack<ZStackFrame>();

            // Simple managers abstracting variable and argument usage
            VariableManager = new VariableManager(Stack, Globals);
            OperandManager = new OperandManager(Manager, Stack, VariableManager);
        }

        public byte PeekNextByte()
        {
            return Memory[Stack.Peek().PC+1];
        }
        public byte PeekPreviousByte()
        {
            return Memory[Stack.Peek().PC-1];
        }

        public byte PeekCurrentByte()
        {
            return Memory[Stack.Peek().PC];
        }
        public byte GetCurrentByteAndInc()
        {
            return Memory[Stack.Peek().PC++];
        }

        public uint GetPackedAddress(ushort address)
        {
            if (Header.Version <= 3)
                return (uint)(address * 2);
            if (Header.Version <= 5)
                return (uint)(address * 4);

            return 0;
        }
        
        public ushort ReadTextAddr { get; set; }
        public ushort ReadParseAddr { get; set; }

        public bool TerminateOnInput { get; set; }
        public bool Running { get; set; }

        public void Restart()
        {
            _restart();
        }
    }
}