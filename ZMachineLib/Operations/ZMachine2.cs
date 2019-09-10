﻿using System;
using System.Collections.Generic;
using System.IO;
using ZMachineLib.Operations.Kind0;
using ZMachineLib.Operations.Kind1;
using ZMachineLib.Operations.Kind2;

namespace ZMachineLib.Operations
{
    public class ZMachine2
    {
        public ZsciiString ZsciiString { get; }
        private readonly IZMachineIo _io;

        internal VersionedOffsets VersionedOffsets;

        internal byte[] Memory;
        internal Stack<ZStackFrame> Stack = new Stack<ZStackFrame>();

        internal byte Version;
        internal ushort Globals;
        internal ushort AbbreviationsTable;
        internal ushort DynamicMemorySize;
        internal ushort ReadTextAddr;
        internal ushort ReadParseAddr;

        internal ushort Pc;
        internal ushort ObjectTable;
        internal ushort Dictionary;
        internal bool TerminateOnInput;

        private Stream _gameFileStream;
        private bool _running;
        private Random _random = new Random();
        private string[] _dictionaryWords;


        private Kind0Operations _kind0Ops;
        // ReSharper disable once CollectionNeverUpdated.Local
        private Kind1Operations _kind1Ops;
        // ReSharper disable once CollectionNeverUpdated.Local
        private Kind2Operations _kind2Ops;

        private readonly Opcode[] _varOpCodes = new Opcode[0x20];
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

        private byte _entryLength;
        private ushort _wordStart;

        public ZMachine2(IZMachineIo io)
        {
            _io = io;
            ZsciiString = new ZsciiString(this);
            InitOpCodes(_varOpCodes);
            InitOpCodes(_extOpCodes);
        }

        private void InitOpCodes(Opcode[] opCodes)
        {
            for (var i = 0; i < opCodes.Length; i++)
                opCodes[i] = _unknownOpCode;
        }

        private void SetupOpCodes()
        {
            _varOpCodes[0x00] = new Opcode {Handler = Call, Name = "CALL(_VS)"};
            _varOpCodes[0x01] = new Opcode {Handler = StoreW, Name = "STOREW"};
            _varOpCodes[0x02] = new Opcode {Handler = StoreB, Name = "STOREB"};
            _varOpCodes[0x03] = new Opcode {Handler = PutProp, Name = "PUT_PROP"};
            _varOpCodes[0x04] = new Opcode {Handler = Read, Name = "READ"};
            _varOpCodes[0x05] = new Opcode {Handler = PrintChar, Name = "PRINT_CHAR"};
            _varOpCodes[0x06] = new Opcode {Handler = PrintNum, Name = "PRINT_NUM"};
            _varOpCodes[0x07] = new Opcode {Handler = Random, Name = "RANDOM"};
            _varOpCodes[0x08] = new Opcode {Handler = Push, Name = "PUSH"};
            _varOpCodes[0x09] = new Opcode {Handler = Pull, Name = "PULL"};
            _varOpCodes[0x0a] = new Opcode {Handler = SplitWindow, Name = "SPLIT_WINDOW"};
            _varOpCodes[0x0b] = new Opcode {Handler = SetWindow, Name = "SET_WINDOW"};
            _varOpCodes[0x0c] = new Opcode {Handler = CallVs2, Name = "CALL_VS2"};
            _varOpCodes[0x0d] = new Opcode {Handler = EraseWindow, Name = "ERASE_WINDOW"};
            _varOpCodes[0x0f] = new Opcode {Handler = SetCursor, Name = "SET_CURSOR"};
            _varOpCodes[0x11] = new Opcode {Handler = SetTextStyle, Name = "SET_TEXT_STYLE"};
            _varOpCodes[0x12] = new Opcode {Handler = BufferMode, Name = "BUFFER_MODE"};
            _varOpCodes[0x13] = new Opcode {Handler = OutputStream, Name = "OUTPUT_STREAM"};
            _varOpCodes[0x15] = new Opcode {Handler = SoundEffect, Name = "SOUND_EFFECT"};
            _varOpCodes[0x16] = new Opcode {Handler = ReadChar, Name = "READ_CHAR"};
            _varOpCodes[0x17] = new Opcode {Handler = ScanTable, Name = "SCAN_TABLE"};
            _varOpCodes[0x18] = new Opcode {Handler = Not, Name = "NOT"};
            _varOpCodes[0x19] = new Opcode {Handler = CallVn, Name = "CALL_VN"};
            _varOpCodes[0x1a] = new Opcode {Handler = CallVn2, Name = "CALL_VN2"};
            _varOpCodes[0x1d] = new Opcode {Handler = CopyTable, Name = "COPY_TABLE"};
            _varOpCodes[0x1e] = new Opcode {Handler = PrintTable, Name = "PRINT_TABLE"};
            _varOpCodes[0x1f] = new Opcode {Handler = CheckArgCount, Name = "CHECK_ARG_COUNT"};

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
            Memory = InitialiseMemoryBuffer(stream);

            ReadHeaderInfo();

            // TODO: set these via IZMachineIO
            Memory[0x01] = 0x01;
            Memory[0x20] = 25;
            Memory[0x21] = 80;

            SetupOpCodes();

            SetupNewOperations();

            ParseDictionary();

            VersionedOffsets = VersionedOffsets.For(Version);

            var zsf = new ZStackFrame {PC = Pc};
            Stack.Push(zsf);
        }

        private byte[] InitialiseMemoryBuffer(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, (int) stream.Length);
            return buffer;
        }

        private void ReadHeaderInfo()
        {
            Version = Memory[HeaderOffsets.VersionOffset];
            Pc = GetWord(HeaderOffsets.InitialPcOffset);
            Dictionary = GetWord(HeaderOffsets.DictionaryOffset);
            ObjectTable = GetWord(HeaderOffsets.ObjectTableOffset);
            Globals = GetWord(HeaderOffsets.GlobalVarOffset);
            DynamicMemorySize = GetWord(HeaderOffsets.StaticMemoryOffset);
            AbbreviationsTable = GetWord(HeaderOffsets.AbbreviationTableOffset);
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
                    opcode = _varOpCodes?[o & 0x1f];

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

        private void ScanTable(List<ushort> args)
        {
            var dest = Memory[Stack.Peek().PC++];
            byte len = 0x02;

            if (args.Count == 4)
                len = (byte) (args[3] & 0x7f);

            for (var i = 0; i < args[2]; i++)
            {
                var addr = (ushort) (args[1] + i * len);
                ushort val;

                if (args.Count == 3 || (args[3] & 0x80) == 0x80)
                    val = GetWord(addr);
                else
                    val = Memory[addr];

                if (val == args[0])
                {
                    StoreWordInVariable(dest, addr);
                    Jump(true);
                    return;
                }
            }

            StoreWordInVariable(dest, 0);
            Jump(false);
        }

        private void CopyTable(List<ushort> args)
        {
            if (args[1] == 0)
            {
                for (var i = 0; i < args[2]; i++)
                    Memory[args[0] + i] = 0;
            }
            else if ((short) args[1] < 0)
            {
                for (var i = 0; i < Math.Abs(args[2]); i++)
                    Memory[args[1] + i] = Memory[args[0] + i];
            }
            else
            {
                for (var i = Math.Abs(args[2]) - 1; i >= 0; i--)
                    Memory[args[1] + i] = Memory[args[0] + i];
            }
        }

        private void PrintTable(List<ushort> args)
        {
            // TODO: print properly

            var s = ZsciiString.GetZsciiString(args[0]);
            _io.Print(s);
            Log.Write($"[{s}]");
        }

        private void PrintNum(List<ushort> args)
        {
            var s = args[0].ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }

        private void PrintChar(List<ushort> args)
        {
            var s = Convert.ToChar(args[0]).ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }

        private void SplitWindow(List<ushort> args)
        {
            _io.SplitWindow(args[0]);
        }

        private void SetWindow(List<ushort> args)
        {
            _io.SetWindow(args[0]);
        }

        private void EraseWindow(List<ushort> args)
        {
            _io.EraseWindow(args[0]);
        }

        private void SetCursor(List<ushort> args)
        {
            _io.SetCursor(args[0], args[1], (ushort) (args.Count == 3 ? args[2] : 0));
        }

        private void SetTextStyle(List<ushort> args)
        {
            _io.SetTextStyle((TextStyle) args[0]);
        }

        private void SetFont(List<ushort> args)
        {
            // TODO

            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, 0);
        }

        private void SoundEffect(List<ushort> args)
        {
            // TODO - the rest of the params

            _io.SoundEffect(args[0]);
        }

        private void BufferMode(List<ushort> args)
        {
            _io.BufferMode(args[0] == 1);
        }

        private void OutputStream(List<ushort> args)
        {
            // TODO
        }

        private void Read(List<ushort> args)
        {
            ReadTextAddr = args[0];
            ReadParseAddr = args[1];

            if (TerminateOnInput)
                _running = false;
            else
            {
                var max = Memory[ReadTextAddr];
                var input = _io.Read(max);
                FinishRead(input);
            }
        }

        public void FinishRead(string input)
        {
            if (input != null && ReadTextAddr != 0 && ReadParseAddr != 0)
            {
                int textMax = Memory[ReadTextAddr];
                int wordMax = Memory[ReadParseAddr];

                input = input.ToLower().Substring(0, Math.Min(input.Length, textMax));
                Log.Write($"[{input}]");

                var ix = 1;

                if (Version >= 5)
                    Memory[ReadTextAddr + ix++] = (byte) input.Length;

                for (var j = 0; j < input.Length; j++, ix++)
                    Memory[ReadTextAddr + ix] = (byte) input[j];

                if (Version < 5)
                    Memory[ReadTextAddr + ++ix] = 0;

                var tokenised = input.Split(' ');

                Memory[ReadParseAddr + 1] = (byte) tokenised.Length;

                var len = (Version <= 3) ? 6 : 9;
                var last = 0;
                var max = Math.Min(tokenised.Length, wordMax);

                for (var i = 0; i < max; i++)
                {
                    if (tokenised[i].Length > len)
                        tokenised[i] = tokenised[i].Substring(0, len);

                    var wordIndex = (ushort) (Array.IndexOf(_dictionaryWords, tokenised[i]));
                    var addr = (ushort) (wordIndex == 0xffff ? 0 : _wordStart + wordIndex * _entryLength);
                    StoreWord((ushort) (ReadParseAddr + 2 + i * 4), addr);
                    Memory[ReadParseAddr + 4 + i * 4] = (byte) tokenised[i].Length;
                    var index = input.IndexOf(tokenised[i], last, StringComparison.Ordinal);
                    Memory[ReadParseAddr + 5 + i * 4] = (byte) (index + (Version < 5 ? 1 : 2));
                    last = index + tokenised[i].Length;
                }

                if (Version >= 5)
                {
                    var dest = Memory[Stack.Peek().PC++];
                    StoreByteInVariable(dest, 10);
                }

                ReadTextAddr = 0;
                ReadParseAddr = 0;
            }
        }

        private void ReadChar(List<ushort> args)
        {
            var key = _io.ReadChar();

            var dest = Memory[Stack.Peek().PC++];
            StoreByteInVariable(dest, (byte) key);
        }
        private void PutProp(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            var prop = GetPropertyHeaderAddress(args[0]);
            var size = Memory[prop];
            prop += (ushort) (size * 2 + 1);

            while (Memory[prop] != 0x00)
            {
                var propInfo = Memory[prop++];
                byte len;
                if (Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte) (Memory[prop++] & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte) ((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

                var propNum = (byte) (propInfo & (Version <= 3 ? 0x1f : 0x3f));
                if (propNum == args[1])
                {
                    if (len == 1)
                        Memory[prop + 1] = (byte) args[2];
                    else
                        StoreWord(prop, args[2]);

                    break;
                }

                prop += len;
            }
        }

        private void StoreB(List<ushort> args)
        {
            var addr = (ushort) (args[0] + args[1]);
            Memory[addr] = (byte) args[2];
        }

        private void StoreW(List<ushort> args)
        {
            var addr = (ushort) (args[0] + 2 * args[1]);
            StoreWord(addr, args[2]);
        }

        private void Jump(bool flag)
        {
            bool branch;

            var offset = Memory[Stack.Peek().PC++];
            short newOffset;

            if ((offset & 0x80) == 0x80)
            {
                Log.Write(" [TRUE] ");
                branch = true;
            }
            else
            {
                Log.Write(" [FALSE] ");
                branch = false;
            }

            var executeBranch = branch && flag || !branch && !flag;

            if ((offset & 0x40) == 0x40)
            {
                offset = (byte) (offset & 0x3f);

                if (offset == 0 && executeBranch)
                {
                    Log.Write(" RFALSE ");
                    _kind0Ops.RFalse.Execute(null);
//                    _kind0Ops.RFalse.Execute(null);
                    return;
                }

                if (offset == 1 && executeBranch)
                {
                    Log.Write(" RTRUE ");
                    _kind0Ops.RTrue.Execute(null);
//                    _kind0Ops.RTrue.Execute(null);
                    return;
                }

                newOffset = (short) (offset - 2);
            }
            else
            {
                var offset2 = Memory[Stack.Peek().PC++];
                var final = (ushort) ((offset & 0x3f) << 8 | offset2);

                // this is a 14-bit number, so set the sign bit properly because we can jump backwards
                if ((final & 0x2000) == 0x2000)
                    final |= 0xc000;

                newOffset = (short) (final - 2);
            }

            if (executeBranch)
                Stack.Peek().PC += (uint) newOffset;

            Log.Write($"-> {Stack.Peek().PC:X5}");
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

        private void Random(List<ushort> args)
        {
            ushort val = 0;

            if ((short) args[0] <= 0)
                _random = new Random(-args[0]);
            else
                val = (ushort) (_random.Next(0, args[0]) + 1);

            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, val);
        }

        private void Not(List<ushort> args)
        {
            var dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort) ~args[0]);
        }

        private void Call(List<ushort> args)
        {
            Call(args, true);
        }

        private void Call(List<ushort> args, bool storeResult)
        {
            if (args[0] == 0)
            {
                if (storeResult)
                {
                    var dest = Memory[Stack.Peek().PC++];
                    StoreWordInVariable(dest, 0);
                }

                return;
            }

            var pc = GetPackedAddress(args[0]);
            Log.Write($"New PC: {pc:X5}");

            var zsf = new ZStackFrame {PC = pc, StoreResult = storeResult};
            Stack.Push(zsf);

            var count = Memory[Stack.Peek().PC++];

            if (Version <= 4)
            {
                for (var i = 0; i < count; i++)
                {
                    zsf.Variables[i] = GetWord(Stack.Peek().PC);
                    Stack.Peek().PC += 2;
                }
            }

            for (var i = 0; i < args.Count - 1; i++)
                zsf.Variables[i] = args[i + 1];

            zsf.ArgumentCount = args.Count - 1;
        }

        private void CallVn(List<ushort> args)
        {
            Call(args, false);
        }

        private void CallVn2(List<ushort> args)
        {
            Call(args, false);
        }

        private void CallVs2(List<ushort> args)
        {
            Call(args, true);
        }

        private void CheckArgCount(List<ushort> args)
        {
            Jump(args[0] <= Stack.Peek().ArgumentCount);
        }

        private void Push(List<ushort> args)
        {
            Stack.Peek().RoutineStack.Push(args[0]);
        }

        private void Pull(List<ushort> args)
        {
            var val = Stack.Peek().RoutineStack.Pop();
            StoreWordInVariable((byte) args[0], val, false);
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

        private void StoreByteInVariable(byte dest, byte value)
        {
            if (dest == 0)
            {
                Log.Write($"-> SP ({value:X4}), ");
                Stack.Peek().RoutineStack.Push(value);
            }
            else if (dest < 0x10)
            {
                Log.Write($"-> L{dest - 1:X2} ({value:X4}), ");
                Stack.Peek().Variables[dest - 1] = value;
            }
            else
            {
                // this still gets written as a word...write the byte to addr+1
                Log.Write($"-> G{dest - 0x10:X2} ({value:X4}), ");
                Memory[Globals + 2 * (dest - 0x10)] = 0;
                Memory[Globals + 2 * (dest - 0x10) + 1] = value;
            }
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
                StoreWord((ushort) (Globals + 2 * (dest - 0x10)), value);
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
                val = GetWord((ushort) (Globals + 2 * (variable - 0x10)));
                Log.Write($"G{variable - 0x10:X2} ({val:X4}), ");
            }

            return val;
        }

        private ushort GetWord(uint address)
        {
            return (ushort) (Memory[address] << 8 | Memory[address + 1]);
        }

        private void StoreWord(ushort address, ushort value)
        {
            Memory[address + 0] = (byte) (value >> 8);
            Memory[address + 1] = (byte) value;
        }

        private uint GetPackedAddress(ushort address)
        {
            if (Version <= 3)
                return (uint) (address * 2);
            if (Version <= 5)
                return (uint) (address * 4);

            return 0;
        }

        private ushort GetObjectAddress(ushort obj)
        {
            var objectAddr = (ushort) (ObjectTable + VersionedOffsets.PropertyDefaultTableSize + (obj - 1) * VersionedOffsets.ObjectSize);
            return objectAddr;
        }

        private ushort GetPropertyHeaderAddress(ushort obj)
        {
            var objectAddr = GetObjectAddress(obj);
            var propAddr = (ushort) (objectAddr + VersionedOffsets.Property);
            var prop = GetWord(propAddr);
            return prop;
        }

        private string GetObjectName(ushort obj)
        {
            var s = string.Empty;

            if (obj != 0)
            {
                var addr = GetPropertyHeaderAddress(obj);
                if (Memory[addr] != 0)
                {
                    s = ZsciiString.GetZsciiString((uint)(addr + 1));
                }
            }

            return s;
        }

        private void ParseDictionary()
        {
            var address = Dictionary;

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
    }
}