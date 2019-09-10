using System;
using System.Collections.Generic;
using System.IO;
using ZMachineLib.Operations.Kind0;
using ZMachineLib.Operations.Kind1;
using ZMachineLib.Operations.Kind2;
using ZMachineLib.Operations.KindVar;

namespace ZMachineLib.Operations
{
    public class ZMachine2
    {
        public ZsciiString ZsciiString { get; }
        private readonly IZMachineIo _io;

        internal VersionedOffsets VersionedOffsets;

        internal byte[] Memory;
        internal Stack<ZStackFrame> Stack = new Stack<ZStackFrame>();

        internal ushort ReadTextAddr;
        internal ushort ReadParseAddr;

        internal bool TerminateOnInput;

        private Stream _gameFileStream;
        internal bool _running;
        private System.Random _random = new System.Random();
        internal string[] _dictionaryWords;


        private Kind0Operations _kind0Ops;
        // ReSharper disable once CollectionNeverUpdated.Local
        private Kind1Operations _kind1Ops;
        // ReSharper disable once CollectionNeverUpdated.Local
        private Kind2Operations _kind2Ops;
        // ReSharper disable once CollectionNeverUpdated.Local
        private KindVarOperations _kindVarOps;

        private readonly Opcode[] _extOpCodes = new Opcode[0x20];

        private readonly Opcode _unknownOpCode = new Opcode
        {
            Handler = delegate
            {
                Log.Flush();
                throw new Exception("Unknown OpCode");
            },
            Name = "UNKNOWN"
        };

        internal byte _entryLength;
        internal ushort _wordStart;
        public FileHeader Header { get; private set; }

        public ZMachine2(IZMachineIo io)
        {
            _io = io;
            ZsciiString = new ZsciiString(this);
            InitOpCodes(_extOpCodes);
        }

        private void InitOpCodes(Opcode[] opCodes)
        {
            for (var i = 0; i < opCodes.Length; i++)
                opCodes[i] = _unknownOpCode;
        }

        private void SetupOpCodes()
        {
            _extOpCodes[0x00] = new Opcode {Handler = Save, Name = "SAVE"};
            _extOpCodes[0x01] = new Opcode {Handler = Restore, Name = "RESTORE"};
            _extOpCodes[0x02] = new Opcode {Handler = LogShift, Name = "LOG_SHIFT"};
            _extOpCodes[0x03] = new Opcode {Handler = ArtShift, Name = "ART_SHIFT"};
            _extOpCodes[0x04] = new Opcode {Handler = SetFont, Name = "SET_FONT"};
        }

        public void RunFile(Stream stream, bool terminateOnInput = false)
        {
            _gameFileStream = stream;
            LoadFile(stream);
            Run();
        }

        internal void ReloadFile()
        {
            LoadFile(_gameFileStream);
        }

        private void LoadFile(Stream stream)
        {
            Memory = ReadToMemory(stream);

            Header = ReadHeaderInfo();
            // NOTE: Need header to be read (mainly for the Version) before we can setup the Ops as few of them have header value dependencies
            SetupNewOperations();

            // TODO: set these via IZMachineIO
            Memory[0x01] = 0x01;
            Memory[0x20] = 25;
            Memory[0x21] = 80;

            SetupOpCodes();

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
            _kind0Ops = new Kind0Operations(this, _io);
            RTrue = _kind0Ops[Kind0OpCodes.RTrue];
            RFalse = _kind0Ops[Kind0OpCodes.RFalse];

            _kind1Ops = new Kind1Operations(this, _io);
            _kind2Ops = new Kind2Operations(this, _io);
            _kindVarOps = new KindVarOperations(this, _io);
        }

        public void Run(bool terminateOnInput = false)
        {
            TerminateOnInput = terminateOnInput;

            _running = true;

            while (_running)
            {
                Opcode? opcode = null;
                IOperation operation = null;
                var opKind = OpKinds.Unknown;

                Log.Write($"PC: {Stack.Peek().PC:X5}");
                var o = Memory[Stack.Peek().PC++];
                if (o == 0xbe)
                {
                    o = Memory[Stack.Peek().PC++];
                    opcode = _extOpCodes?[o & 0x1f];
                    // TODO: hack to make this a VAR opcode...
                    o |= 0xc0;
                }
                else if (o < 0x80)
                {
                    _kind2Ops.TryGetValue((Kind2OpCodes)(o & 0x1f), out operation);
                    opKind = OpKinds.Kind2;
                }
                else if (o < 0xb0)
                {
                    _kind1Ops.TryGetValue((Kind1OpCodes)(o & 0x0f), out operation);
                    opKind = OpKinds.Kind1;
                }
                else if (o < 0xc0)
                {
                    _kind0Ops.TryGetValue((Kind0OpCodes) (o & 0x0f), out operation);
                    opKind = OpKinds.Kind0;
                }
                else if (o < 0xe0)
                {
                    _kind2Ops.TryGetValue((Kind2OpCodes)(o & 0x1f), out operation);
                    opKind = OpKinds.Kind2;
                }
                else
                {
                    _kindVarOps.TryGetValue((KindVarOpCodes)(o & 0x1f), out operation);
                    opKind = OpKinds.KindVar;
                }

                Log.Write($" Op ({o:X2}): {opcode?.Name} ");
                var args = GetOperands(o);

                if (operation != null)
                {
                    if (opKind == OpKinds.Kind0)
                    {
                        switch ((Kind0OpCodes)operation.Code)
                        {
                            case Kind0OpCodes.Quit:
                                _running = false;
                                _io.Quit();
                                break;
                            default:
                                operation.Execute(args);
                                break;
                        }
                    }
                    else
                    {
                        operation.Execute(args);
                    }
                }
                else
                {
                    opcode?.Handler(args);
                }
                Log.Flush();
            }
        }

        private void SetFont(List<ushort> args)
        {
            // TODO

            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, 0);
        }

        private void ArtShift(List<ushort> args)
        {
            // keep the sign bit, so make it a short
            var val = (short) args[0];
            if ((short) args[1] > 0)
                val <<= args[1];
            else if ((short) args[1] < 0)
                val >>= -args[1];

            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort) val);
        }

        private void LogShift(List<ushort> args)
        {
            // kill the sign bit, so make it a ushort
            var val = args[0];
            if ((short) args[1] > 0)
                val <<= args[1];
            else if ((short) args[1] < 0)
                val >>= -args[1];

            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, val);
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

        private void StoreWordInVariable(byte dest, ushort value, bool push = true)
        {
            if (dest == 0)
            {
                Log.Write($"-> SP ({value:X4}), ");
                if (!push)
                    Stack.Peek().RoutineStack.Pop();
                Stack.Peek().RoutineStack.Push(value);
            }
            else if (dest < 0x10)
            {
                Log.Write($"-> L{dest - 1:X2} ({value:X4}), ");
                Stack.Peek().Variables[dest - 1] = value;
            }
            else
            {
                Log.Write($"-> G{dest - 0x10:X2} ({value:X4}), ");
                StoreWord((ushort) (Header.Globals + 2 * (dest - 0x10)), value);
            }
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

        private ushort GetWord(uint address) => GetWord(Memory, address);

        public static ushort GetWord(byte[] memory, uint address)
        {
            return (ushort)(memory[address] << 8 | memory[address + 1]);
        }
        private void StoreWord(ushort address, ushort value)
        {
            Memory[address + 0] = (byte) (value >> 8);
            Memory[address + 1] = (byte) value;
        }

        private void ParseDictionary()
        {
            var address = Header.Dictionary;

            var len = Memory[address++];
            address += len;

            _entryLength = Memory[address++];
            var numEntries = GetWord(address);
            address += 2;

            _wordStart = address;

            _dictionaryWords = new string[numEntries];

            for (var i = 0; i < numEntries; i++)
            {
                var wordAddress = (ushort) (address + i * _entryLength);
                var chars = ZsciiString.GetZsciiChar(wordAddress);
                chars.AddRange(ZsciiString.GetZsciiChar((uint) (wordAddress + 2)));
                if (_entryLength == 9)
                    chars.AddRange(ZsciiString.GetZsciiChar((uint) (wordAddress + 4)));
                var s = ZsciiString.DecodeZsciiChars(chars);
                _dictionaryWords[i] = s;
            }
        }

        private void Restore(List<ushort> args) => _kind0Ops.Restore.Execute(args);
        private void Save(List<ushort> args) => _kind0Ops.Save.Execute(args);

        private FileHeader ReadHeaderInfo() => new FileHeader(Memory);

        public class FileHeader
        {
            public byte Version { get; private set; }
            public ushort Pc { get; private set; }
            public ushort Dictionary { get; private set; }
            public ushort ObjectTable { get; private set; }
            public ushort Globals { get; private set; }
            public ushort DynamicMemorySize { get; private set; }
            public ushort AbbreviationsTable { get; private set; }

            public FileHeader(byte[] memory)
            {
                Version = memory[HeaderOffsets.VersionOffset];
                Pc = ZMachine2.GetWord(memory, HeaderOffsets.InitialPcOffset);
                Dictionary = GetWord(memory, HeaderOffsets.DictionaryOffset);
                ObjectTable = GetWord(memory, HeaderOffsets.ObjectTableOffset);
                Globals = GetWord(memory, HeaderOffsets.GlobalVarOffset);
                DynamicMemorySize = GetWord(memory, HeaderOffsets.StaticMemoryOffset);
                AbbreviationsTable = GetWord(memory, HeaderOffsets.AbbreviationTableOffset);
            }



        }
    }
}