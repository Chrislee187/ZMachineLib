using System;
using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Content
{
    public class ZMachineContents
    {
        public byte Version { get; }
        public ZHeader Header { get; }
        public ZDictionary Dictionary { get; }
        public ZAbbreviations Abbreviations { get; }
        public ZObjectTree ObjectTree { get; }
        public MemoryManager MemoryManager { get; }
        public VariableManager VariableManager { get; }
        public OperandManager OperandManager { get; }
        private byte[] DynamicMemory { get; }

        public byte[] Memory { get; }

        public ushort DictionaryWordStart => (ushort) (Header.Dictionary + Dictionary.WordStart);
        public ZMachineContents(byte[] data, Stack<ZStackFrame> stack)
        {
            Memory = data;
            Version = data[0];

            if (Version > 3) throw new NotSupportedException("ZMachine > V3 not currently supported");

            Header = new ZHeader(data.AsSpan(0,31));
            DynamicMemory = data.AsSpan(0, Header.DynamicMemorySize).ToArray();
            Abbreviations = new ZAbbreviations(data.AsSpan(Header.AbbreviationsTable), DynamicMemory);
            Dictionary = new ZDictionary(data.AsSpan(Header.Dictionary), Abbreviations);
            ObjectTree = new ZObjectTree(DynamicMemory, Header, Abbreviations);

            MemoryManager = new MemoryManager(Memory);
            VariableManager = new VariableManager(MemoryManager, Header, stack);
            OperandManager = new OperandManager(MemoryManager, VariableManager, stack);

        }
    }
}