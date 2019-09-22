using System;
using System.Collections.Generic;
using ZMachineLib.Managers;
using ZMachineLib.Operations;
using ZMachineLib.Operations.OPExtended;

namespace ZMachineLib.Content
{
    public interface IZMemory
    {
        Stack<ZStackFrame> Stack { get; }
        ZHeader Header { get; }
        ZDictionary Dictionary { get; }
        ZAbbreviations Abbreviations { get; }
        IReadOnlyDictionary<ushort, ZMachineObject> ObjectTree { get; }
        IMemoryManager Manager { get; }
        IVariableManager VariableManager { get; }
        OperandManager OperandManager { get; }
        VersionedOffsets Offsets { get; }
        ushort DictionaryWordStart { get; }
        byte PeekNextByte();
        byte GetNextByte();
    }

    public class ZMemory : IZMemory
    {
        public Stack<ZStackFrame> Stack { get; }
        public ZHeader Header { get; }
        public ZDictionary Dictionary { get; }
        public ZAbbreviations Abbreviations { get; }
        public IReadOnlyDictionary<ushort, ZMachineObject> ObjectTree { get; }
        public IMemoryManager Manager { get; }
        public IVariableManager VariableManager { get; }
        public OperandManager OperandManager { get; }

        public byte[] Memory { get; }
        public VersionedOffsets Offsets { get; private set; }
        public ushort DictionaryWordStart => (ushort) (Header.Dictionary + Dictionary.WordStart);
        public ZMemory(byte[] data, Stack<ZStackFrame> stack)
        {
            Stack = stack;
            Memory = data;
            var version = data[0];
            Offsets = VersionedOffsets.For(version);
            Manager = new MemoryManager(Memory);

            if (version > 3) throw new NotSupportedException("ZMachine > V3 not currently supported");

            Header = new ZHeader(data.AsSpan(0,31));
            Offsets = VersionedOffsets.For(Header.Version);

            Abbreviations = new ZAbbreviations(
                data.AsSpan(Header.AbbreviationsTable), 
                data.AsSpan(0, Header.DynamicMemorySize).ToArray());

            Dictionary = new ZDictionary(data.AsSpan(Header.Dictionary), Abbreviations);
            ObjectTree = new ZObjectTree(Header, Abbreviations, Manager);

            VariableManager = new VariableManager(Manager, Header, stack);
            OperandManager = new OperandManager(Manager, VariableManager, stack);
        }


        public byte PeekNextByte()
        {
            return Memory[Stack.Peek().PC];
        }
        public byte GetNextByte()
        {
            return Memory[Stack.Peek().PC++];
        }
        private KindExtOperations _extendedOperations;
        private Operations.Operations _operations;
        
        public (byte opCode, OpCodes opCodeEnum, IOperation operation) GetOperation(byte opCode)
        {
            //NOTE: http://inform-fiction.org/zmachine/standards/z1point1/sect14.html
            IOperation operation;
            OpCodes opCodeEnum;
            if (opCode == (byte)OpCodes.Extended) // 0OP:190 - special op, indicates next byte contains Extended Op
            {
                opCodeEnum = OpCodes.Extended;
                opCode = GetNextByte();
                _extendedOperations.TryGetValue((KindExtOpCodes)(opCode & 0x1f), out operation);
                // TODO: hack to make this a VAR opcode...
                opCode |= 0xc0;

                Log.Write($" Ext ");
            }
            else
            {
                opCodeEnum = opCode.ToOpCode();

                _operations.TryGetValue(opCodeEnum, out operation);
            }

            return (opCode, opCodeEnum, operation);
        }
    }
}