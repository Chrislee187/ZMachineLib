﻿using System;
using System.Collections.Generic;
using System.IO;
using ZMachineLib.Operations;
using ZMachineLib.Operations.Kind0;
using ZMachineLib.Operations.Kind1;
using ZMachineLib.Operations.Kind2;
using ZMachineLib.Operations.KindExt;
using ZMachineLib.Operations.KindVar;

namespace ZMachineLib
{
    public class ZMachine2
    {
        internal FileHeader Header { get; private set; }
        internal VersionedOffsets VersionedOffsets;

        internal byte[] Memory;
        internal Stack<ZStackFrame> Stack = new Stack<ZStackFrame>();
        
        internal ZsciiString ZsciiString { get; }

        internal ushort ReadTextAddr;
        internal ushort ReadParseAddr;

        internal bool TerminateOnInput;
        internal bool Running;
        internal string[] DictionaryWords;
        internal byte EntryLength;
        internal ushort WordStart;

        private Stream _gameFileStream;
        private readonly IUserIo _io;

        // ReSharper disable once CollectionNeverUpdated.Local
        private Kind0Operations _kind0Ops;
        // ReSharper disable once CollectionNeverUpdated.Local
        private Kind1Operations _kind1Ops;
        // ReSharper disable once CollectionNeverUpdated.Local
        private Kind2Operations _kind2Ops;
        // ReSharper disable once CollectionNeverUpdated.Local
        private KindVarOperations _kindVarOps;
        // ReSharper disable once CollectionNeverUpdated.Local
        private KindExtOperations _kindExtOps;
        private readonly IFileIo _fileIo;

        public ZMachine2(IUserIo io, IFileIo fileIo)
        {
            _fileIo = fileIo;
            _io = io;
            ZsciiString = new ZsciiString(this);
        }

        public void RunFile(Stream stream, bool terminateOnInput = false)
        {
            _gameFileStream = stream;
            LoadFile(stream);
            Run();
        }

        public void RunFile(string filename)
        {
            var fileStream = File.OpenRead(filename);
            RunFile(fileStream);

        }

        internal void ReloadFile()
        {
            LoadFile(_gameFileStream);
        }

        private void LoadFile(Stream stream)
        {
            Memory = ReadToMemory(stream);

//            Header = ReadHeaderInfo();
            Header = ReadHeaderInfo();
            // NOTE: Need header to be read (mainly for the Version) before we can setup the Ops as few of them have header value dependencies
            SetupNewOperations();
#if DEBUG
            Console.WriteLine($"File version: {Header.Version}");
#endif


            // TODO: set these via IZMachineIO
            Memory[0x01] = 0x01;
            Memory[0x20] = 25;
            Memory[0x21] = 80;

            ParseDictionary();

            VersionedOffsets = VersionedOffsets.For(Header.Version);

            var zsf = new ZStackFrame {PC = Header.Pc };
            Stack.Push(zsf);
        }

        private byte[] ReadToMemory(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, (int) stream.Length);
            return buffer;
        }

        public IOperation RTrue { get; private set; }
        public IOperation RFalse { get; private set; }

        private void SetupNewOperations()
        {
            _kind0Ops = new Kind0Operations(this, _io, _fileIo);
            RTrue = _kind0Ops[Kind0OpCodes.RTrue];
            RFalse = _kind0Ops[Kind0OpCodes.RFalse];

            _kind1Ops = new Kind1Operations(this, _io);
            _kind2Ops = new Kind2Operations(this, _io);
            _kindVarOps = new KindVarOperations(this, _io);
            _kindExtOps = new KindExtOperations(this, _kind0Ops);
        }

        public void Run(bool terminateOnInput = false)
        {
            TerminateOnInput = terminateOnInput;

            Running = true;

            while (Running)
            {
                Log.Write($"PC: {Stack.Peek().PC:X5}");
                var o = Memory[Stack.Peek().PC++];
                IOperation operation;
                if (o == 0xbe)
                {
                    o = Memory[Stack.Peek().PC++];
                    _kindExtOps.TryGetValue((KindExtOpCodes)(o & 0x1f), out operation);
                    // TODO: hack to make this a VAR opcode...
                    o |= 0xc0;

                    Log.Write($" Ext ");

                }
                else if (o < 0x80)
                {
                    _kind2Ops.TryGetValue((Kind2OpCodes)(o & 0x1f), out operation);
                    Log.Write($" 2Op(0x80) ");
                }
                else if (o < 0xb0)
                {
                    _kind1Ops.TryGetValue((Kind1OpCodes)(o & 0x0f), out operation);
                    Log.Write($" 1Op ");
                }
                else if (o < 0xc0)
                {
                    _kind0Ops.TryGetValue((Kind0OpCodes)(o & 0x0f), out operation);
                    Log.Write($" 0Op ");
                }
                else if (o < 0xe0)
                {
                    _kind2Ops.TryGetValue((Kind2OpCodes)(o & 0x1f), out operation);
                    Log.Write($" 2Op(0xe0) ");
                }
                else
                {
                    _kindVarOps.TryGetValue((KindVarOpCodes)(o & 0x1f), out operation);
                    Log.Write($" Var ");
                }

                Log.WriteLine($" Op Code ({operation?.Code:X2})");
                var args = GetOperands(o);

                if (operation == null) throw new Exception($"No operation found!");

                operation.Execute(args);

                Log.Flush();
            }
        }

        private List<ushort> GetOperands(byte opcode)
        {
            var args = new List<ushort>();
            ushort arg;

            // Variable
            if ((opcode & 0xc0) == 0xc0)
            {
                var types = Memory[Stack.Peek().PC++];
                byte types2 = 0;

                if (opcode == 0xec || opcode == 0xfa)
                    types2 = Memory[Stack.Peek().PC++];

                GetVariableOperands(types, args);
                if (opcode == 0xec || opcode == 0xfa)
                    GetVariableOperands(types2, args);
            }
            // Short
            else if ((opcode & 0x80) == 0x80)
            {
                var type = (byte) (opcode >> 4 & 0x03);
                arg = GetOperand((OperandType) type);
                args.Add(arg);
            }
            // Long
            else
            {
                arg = GetOperand((opcode & 0x40) == 0x40 ? OperandType.Variable : OperandType.SmallConstant);
                args.Add(arg);

                arg = GetOperand((opcode & 0x20) == 0x20 ? OperandType.Variable : OperandType.SmallConstant);
                args.Add(arg);
            }

            return args;
        }

        private void GetVariableOperands(byte types, List<ushort> args)
        {
            for (var i = 6; i >= 0; i -= 2)
            {
                var type = (byte) ((types >> i) & 0x03);

                // omitted
                if (type == 0x03)
                    break;

                var arg = GetOperand((OperandType) type);
                args.Add(arg);
            }
        }

        private ushort GetOperand(OperandType type)
        {
            ushort arg = 0;

            switch (type)
            {
                case OperandType.LargeConstant:
                    arg = GetWord(Stack.Peek().PC);
                    Stack.Peek().PC += 2;
                    Log.Write($"#{arg:X4}, ");
                    break;
                case OperandType.SmallConstant:
                    arg = Memory[Stack.Peek().PC++];
                    Log.Write($"#{arg:X2}, ");
                    break;
                case OperandType.Variable:
                    var b = Memory[Stack.Peek().PC++];
                    arg = GetVariable(b);
                    break;
            }

            return arg;
        }

        private ushort GetVariable(byte variable, bool pop = true)
        {
            ushort val;

            if (variable == 0)
            {
                if (pop)
                    val = Stack.Peek().RoutineStack.Pop();
                else
                    val = Stack.Peek().RoutineStack.Peek();
                Log.Write($"SP ({val:X4}), ");
            }
            else if (variable < 0x10)
            {
                val = Stack.Peek().Variables[variable - 1];
                Log.Write($"L{variable - 1:X2} ({val:X4}), ");
            }
            else
            {
                val = GetWord((ushort) (Header.Globals + 2 * (variable - 0x10)));
                Log.Write($"G{variable - 0x10:X2} ({val:X4}), ");
            }

            return val;
        }

        public ushort GetWord(uint address) => GetWord(Memory, address);
        public static ushort GetWord(byte[] memory, uint address) => (ushort)(memory[address] << 8 | memory[address + 1]);

        private void ParseDictionary()
        {
            var address = Header.Dictionary;

            var len = Memory[address++];
            address += len;

            EntryLength = Memory[address++];
            var numEntries = GetWord(address);
            address += 2;

            WordStart = address;

            DictionaryWords = new string[numEntries];

            for (var i = 0; i < numEntries; i++)
            {
                var wordAddress = (ushort) (address + i * EntryLength);
                var chars = ZsciiString.GetZsciiChar(wordAddress);
                chars.AddRange(ZsciiString.GetZsciiChar((uint) (wordAddress + 2)));
                if (EntryLength == 9)
                    chars.AddRange(ZsciiString.GetZsciiChar((uint) (wordAddress + 4)));
                var s = ZsciiString.DecodeZsciiChars(chars);
                DictionaryWords[i] = s;
            }
        }

        private FileHeader ReadHeaderInfo() => new FileHeader(Memory);
        public class FileHeader
        {
            public byte Version { get; }
            public ushort Pc { get; }
            public ushort Dictionary { get; }
            public ushort ObjectTable { get; }
            public ushort Globals { get; }
            public ushort DynamicMemorySize { get; }
            public ushort AbbreviationsTable { get; }

            public FileHeader(byte[] memory)
            {
                Version = memory[HeaderOffsets.Version];
                Pc = GetWord(memory, HeaderOffsets.InitialPc);
                Dictionary = GetWord(memory, HeaderOffsets.Dictionary);
                ObjectTable = GetWord(memory, HeaderOffsets.ObjectTable);
                Globals = GetWord(memory, HeaderOffsets.GlobalVar);
                DynamicMemorySize = GetWord(memory, HeaderOffsets.StaticMemory);
                AbbreviationsTable = GetWord(memory, HeaderOffsets.AbbreviationTable);
            }
        }
        // https://inform-fiction.org/zmachine/standards/z1point0/sect11.html
        public struct FileHeader2
        {
            public byte Version;                            // 0x00
            public ushort Flags1;                           // 0x01
            public byte Unknown1;                           // 0x03
            public ushort HighMemoryBaseAddress;            // 0x04
            public ushort ProgramCounter;                   // 0x06 (NB. Packed address of initial main routine in >= V6)
            public ushort Dictionary;                  // 0x08
            public ushort ObjectTable;                      // 0x0a
            public ushort Globals;                          // 0x0c
            public ushort StaticMemoryBaseAddress;          // 0x0e
            public ushort Flags2;                           // 0x10
            public ushort Unknown2;                         // 0x12
            public ushort Unknown3;                         // 0x14
            public ushort Unknown4;                         // 0x16
            public ushort AbbreviationsTable;               // 0x18
            public ushort LengthOfFile;                     // 0x1A
            public ushort ChecksumOfFile;                   // 0x1C
            public byte InterpreterNumber;                  // 0x1E
            public byte InterpreterNumberVersion;           // 0x1F

            public ushort Pc => ProgramCounter;
            public ushort DynamicMemorySize => StaticMemoryBaseAddress;
        }
    }
}