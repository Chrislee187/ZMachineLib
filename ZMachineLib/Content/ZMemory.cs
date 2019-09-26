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

            Stack = new Stack<ZStackFrame>();

            Offsets = VersionedOffsets.For(Header.Version);
            Manager = new MemoryManager(Memory);

            Offsets = VersionedOffsets.For(Header.Version);

            Abbreviations = new ZAbbreviations(
                data.AsSpan(Header.AbbreviationsTable), 
                data.AsSpan(0, Header.DynamicMemorySize).ToArray());

            Dictionary = new ZDictionary(data.AsSpan(Header.Dictionary), Abbreviations);
            ObjectTree = new ZObjectTree(Header, Abbreviations, Manager);

            VariableManager = new VariableManager(Manager, Header, Stack);
            OperandManager = new OperandManager(Manager, VariableManager, Stack);
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


        private bool _running;
        public bool TerminateOnInput { get; set; }

        public ushort ReadTextAddr { get; set; }
        public ushort ReadParseAddr { get; set; }

        public bool Running
        {
            get => _running;
            set => _running = value;
        }

        public void Restart()
        {
            _restart();
        }
    }
}