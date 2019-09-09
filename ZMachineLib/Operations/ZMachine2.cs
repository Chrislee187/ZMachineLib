using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZMachineLib.Operations.Kind0;

namespace ZMachineLib.Operations
{
    public class ZMachine2
    {
        private readonly ZsciiString _zsciiString;
        private readonly IZMachineIO _io;

        private VersionOffsets _versionOffsets;
        private Stream _file;
        private bool _running;
        private Random _random = new Random();
        private string[] _dictionaryWords;

        internal byte[] Memory;
        internal Stack<ZStackFrame> Stack = new Stack<ZStackFrame>();

        internal byte Version;
        internal ushort Globals;
        internal ushort AbbreviationsTable;
        internal ushort DynamicMemorySize;
        internal ushort ReadTextAddr;
        internal ushort ReadParseAddr;

        private ushort _pc;
        private ushort _objectTable;
        private ushort _dictionary;
        private bool _terminateOnInput;


        private Kind0Operations _kind0Ops;
        private Kind1Operations _kind1Ops;
        private readonly Opcode[] _1Opcodes = new Opcode[0x10];
        private readonly Opcode[] _2Opcodes = new Opcode[0x20];
        private readonly Opcode[] _varOpcodes = new Opcode[0x20];
        private readonly Opcode[] _extOpcodes = new Opcode[0x20];

        private readonly Opcode UnknownOpcode = new Opcode
        {
            Handler = delegate
            {
                Log.Flush();
                throw new Exception("Unknown opcode");
            },
            Name = "UNKNOWN"
        };

        private byte _entryLength;
        private ushort _wordStart;

        public ZMachine2(IZMachineIO io)
        {
            _io = io;
            _zsciiString = new ZsciiString(this);

            InitOpcodes(_1Opcodes);
            InitOpcodes(_2Opcodes);
            InitOpcodes(_varOpcodes);
            InitOpcodes(_extOpcodes);
        }

        private void InitOpcodes(Opcode[] opcodes)
        {
            for (int i = 0; i < opcodes.Length; i++)
                opcodes[i] = UnknownOpcode;
        }

        private void SetupOpcodes()
        {
            _1Opcodes[0x00] = new Opcode {Handler = Jz, Name = "JZ"};
            _1Opcodes[0x01] = new Opcode {Handler = GetSibling, Name = "GET_SIBLING"};
            _1Opcodes[0x02] = new Opcode {Handler = GetChild, Name = "GET_CHILD"};
            _1Opcodes[0x03] = new Opcode {Handler = GetParent, Name = "GET_PARENT"};
            _1Opcodes[0x04] = new Opcode {Handler = GetPropLen, Name = "GET_PROP_LEN"};
            _1Opcodes[0x05] = new Opcode {Handler = Inc, Name = "INC"};
            _1Opcodes[0x06] = new Opcode {Handler = Dec, Name = "DEC"};
            _1Opcodes[0x07] = new Opcode {Handler = PrintAddr, Name = "PRINT_ADDR"};
            _1Opcodes[0x08] = new Opcode {Handler = Call1S, Name = "CALL_1S"};
            _1Opcodes[0x09] = new Opcode {Handler = RemoveObj, Name = "REMOVE_OBJ"};
            _1Opcodes[0x0a] = new Opcode {Handler = PrintObj, Name = "PRINT_OBJ"};
            _1Opcodes[0x0b] = new Opcode {Handler = Ret, Name = "RET"};
            _1Opcodes[0x0c] = new Opcode {Handler = Jump, Name = "JUMP"};
            _1Opcodes[0x0d] = new Opcode {Handler = PrintPAddr, Name = "PRINT_PADDR"};
            _1Opcodes[0x0e] = new Opcode {Handler = Load, Name = "LOAD"};
            if (Version <= 4)
                _1Opcodes[0x0f] = new Opcode {Handler = Not, Name = "NOT"};
            else
                _1Opcodes[0x0f] = new Opcode {Handler = Call1N, Name = "CALL_1N"};

            _2Opcodes[0x01] = new Opcode {Handler = Je, Name = "JE"};
            _2Opcodes[0x02] = new Opcode {Handler = Jl, Name = "JL"};
            _2Opcodes[0x03] = new Opcode {Handler = Jg, Name = "JG"};
            _2Opcodes[0x04] = new Opcode {Handler = DecCheck, Name = "DEC_CHECK"};
            _2Opcodes[0x05] = new Opcode {Handler = IncCheck, Name = "INC_CHECK"};
            _2Opcodes[0x06] = new Opcode {Handler = Jin, Name = "JIN"};
            _2Opcodes[0x07] = new Opcode {Handler = Test, Name = "TEST"};
            _2Opcodes[0x08] = new Opcode {Handler = Or, Name = "OR"};
            _2Opcodes[0x09] = new Opcode {Handler = And, Name = "AND"};
            _2Opcodes[0x0a] = new Opcode {Handler = TestAttribute, Name = "TEST_ATTR"};
            _2Opcodes[0x0b] = new Opcode {Handler = SetAttribute, Name = "SET_ATTR"};
            _2Opcodes[0x0c] = new Opcode {Handler = ClearAttribute, Name = "CLEAR_ATTR"};
            _2Opcodes[0x0d] = new Opcode {Handler = Store, Name = "STORE"};
            _2Opcodes[0x0e] = new Opcode {Handler = InsertObj, Name = "INSERT_OBJ"};
            _2Opcodes[0x0f] = new Opcode {Handler = LoadW, Name = "LOADW"};
            _2Opcodes[0x10] = new Opcode {Handler = LoadB, Name = "LOADB"};
            _2Opcodes[0x11] = new Opcode {Handler = GetProp, Name = "GET_PROP"};
            _2Opcodes[0x12] = new Opcode {Handler = GetPropAddr, Name = "GET_PROP_ADDR"};
            _2Opcodes[0x13] = new Opcode {Handler = GetNextProp, Name = "GET_NEXT_PROP"};
            _2Opcodes[0x14] = new Opcode {Handler = Add, Name = "ADD"};
            _2Opcodes[0x15] = new Opcode {Handler = Sub, Name = "SUB"};
            _2Opcodes[0x16] = new Opcode {Handler = Mul, Name = "MUL"};
            _2Opcodes[0x17] = new Opcode {Handler = Div, Name = "DIV"};
            _2Opcodes[0x18] = new Opcode {Handler = Mod, Name = "MOD"};
            _2Opcodes[0x19] = new Opcode {Handler = Call2S, Name = "CALL_2S"};
            _2Opcodes[0x1a] = new Opcode {Handler = Call2N, Name = "CALL_2N"};
            _2Opcodes[0x1b] = new Opcode {Handler = SetColor, Name = "SET_COLOR"};

            _varOpcodes[0x00] = new Opcode {Handler = Call, Name = "CALL(_VS)"};
            _varOpcodes[0x01] = new Opcode {Handler = StoreW, Name = "STOREW"};
            _varOpcodes[0x02] = new Opcode {Handler = StoreB, Name = "STOREB"};
            _varOpcodes[0x03] = new Opcode {Handler = PutProp, Name = "PUT_PROP"};
            _varOpcodes[0x04] = new Opcode {Handler = Read, Name = "READ"};
            _varOpcodes[0x05] = new Opcode {Handler = PrintChar, Name = "PRINT_CHAR"};
            _varOpcodes[0x06] = new Opcode {Handler = PrintNum, Name = "PRINT_NUM"};
            _varOpcodes[0x07] = new Opcode {Handler = Random, Name = "RANDOM"};
            _varOpcodes[0x08] = new Opcode {Handler = Push, Name = "PUSH"};
            _varOpcodes[0x09] = new Opcode {Handler = Pull, Name = "PULL"};
            _varOpcodes[0x0a] = new Opcode {Handler = SplitWindow, Name = "SPLIT_WINDOW"};
            _varOpcodes[0x0b] = new Opcode {Handler = SetWindow, Name = "SET_WINDOW"};
            _varOpcodes[0x0c] = new Opcode {Handler = CallVS2, Name = "CALL_VS2"};
            _varOpcodes[0x0d] = new Opcode {Handler = EraseWindow, Name = "ERASE_WINDOW"};
            _varOpcodes[0x0f] = new Opcode {Handler = SetCursor, Name = "SET_CURSOR"};
            _varOpcodes[0x11] = new Opcode {Handler = SetTextStyle, Name = "SET_TEXT_STYLE"};
            _varOpcodes[0x12] = new Opcode {Handler = BufferMode, Name = "BUFFER_MODE"};
            _varOpcodes[0x13] = new Opcode {Handler = OutputStream, Name = "OUTPUT_STREAM"};
            _varOpcodes[0x15] = new Opcode {Handler = SoundEffect, Name = "SOUND_EFFECT"};
            _varOpcodes[0x16] = new Opcode {Handler = ReadChar, Name = "READ_CHAR"};
            _varOpcodes[0x17] = new Opcode {Handler = ScanTable, Name = "SCAN_TABLE"};
            _varOpcodes[0x18] = new Opcode {Handler = Not, Name = "NOT"};
            _varOpcodes[0x19] = new Opcode {Handler = CallVN, Name = "CALL_VN"};
            _varOpcodes[0x1a] = new Opcode {Handler = CallVN2, Name = "CALL_VN2"};
            _varOpcodes[0x1d] = new Opcode {Handler = CopyTable, Name = "COPY_TABLE"};
            _varOpcodes[0x1e] = new Opcode {Handler = PrintTable, Name = "PRINT_TABLE"};
            _varOpcodes[0x1f] = new Opcode {Handler = CheckArgCount, Name = "CHECK_ARG_COUNT"};

            _extOpcodes[0x00] = new Opcode {Handler = Save, Name = "SAVE"};
            _extOpcodes[0x01] = new Opcode {Handler = Restore, Name = "RESTORE"};
            _extOpcodes[0x02] = new Opcode {Handler = LogShift, Name = "LOG_SHIFT"};
            _extOpcodes[0x03] = new Opcode {Handler = ArtShift, Name = "ART_SHIFT"};
            _extOpcodes[0x04] = new Opcode {Handler = SetFont, Name = "SET_FONT"};
        }

        public void LoadFile(Stream stream)
        {
            _file = stream;

            Memory = InitialiseMemoryBuffer(stream);

            ReadHeaderInfo();

            // TODO: set these via IZMachineIO
            Memory[0x01] = 0x01;
            Memory[0x20] = 25;
            Memory[0x21] = 80;

            SetupOpcodes();

            SetupNewOperations();

            ParseDictionary();

            _versionOffsets = VersionOffsets.For(Version);

            ZStackFrame zsf = new ZStackFrame {PC = _pc};
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
            _pc = GetWord(HeaderOffsets.InitialPCOffset);
            _dictionary = GetWord(HeaderOffsets.DictionaryOffset);
            _objectTable = GetWord(HeaderOffsets.ObjectTableOffset);
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
        }

        public void Run(bool terminateOnInput = false)
        {
            _terminateOnInput = terminateOnInput;

            _running = true;

            while (_running)
            {
                Opcode? opcode;
                IOperation operation = null;

                Log.Write($"PC: {Stack.Peek().PC:X5}");
                byte o = Memory[Stack.Peek().PC++];
                if (o == 0xbe)
                {
                    o = Memory[Stack.Peek().PC++];
                    opcode = _extOpcodes?[o & 0x1f];
                    // TODO: hack to make this a VAR opcode...
                    o |= 0xc0;
                }
                else if (o < 0x80)
                    opcode = _2Opcodes?[o & 0x1f];
                else if (o < 0xb0)
                {
                    opcode = _1Opcodes?[o & 0x0f];
                    _kind1Ops.TryGetValue((Kind1OpCodes)(o & 0x0f), out operation);
                }
                else if (o < 0xc0)
                {
                    opcode = null;
                    _kind0Ops.TryGetValue((Kind0OpCodes) (o & 0x0f), out operation);
                }
                else if (o < 0xe0)
                    opcode = _2Opcodes?[o & 0x1f];
                else
                    opcode = _varOpcodes?[o & 0x1f];

                Log.Write($" Op ({o:X2}): {opcode?.Name} ");
                var args = GetOperands(o);

                if (operation != null)
                {
                    switch (operation.Code)
                    {
                        case Kind0OpCodes.Restart:
                            LoadFile(_file);
                            break;
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
                    opcode?.Handler(args);
                }
                Log.Flush();
            }
        }

        private void ScanTable(List<ushort> args)
        {
            byte dest = Memory[Stack.Peek().PC++];
            byte len = 0x02;

            if (args.Count == 4)
                len = (byte) (args[3] & 0x7f);

            for (int i = 0; i < args[2]; i++)
            {
                ushort addr = (ushort) (args[1] + i * len);
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
                for (int i = 0; i < args[2]; i++)
                    Memory[args[0] + i] = 0;
            }
            else if ((short) args[1] < 0)
            {
                for (int i = 0; i < Math.Abs(args[2]); i++)
                    Memory[args[1] + i] = Memory[args[0] + i];
            }
            else
            {
                for (int i = Math.Abs(args[2]) - 1; i >= 0; i--)
                    Memory[args[1] + i] = Memory[args[0] + i];
            }
        }

        private void PrintTable(List<ushort> args)
        {
            // TODO: print properly

            string s = _zsciiString.GetZsciiString(args[0]);
            _io.Print(s);
            Log.Write($"[{s}]");
        }

        private void PrintNum(List<ushort> args)
        {
            string s = args[0].ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }

        private void PrintChar(List<ushort> args)
        {
            string s = Convert.ToChar(args[0]).ToString();
            _io.Print(s);
            Log.Write($"[{s}]");
        }

        private void PrintObj(List<ushort> args)
        {
            ushort addr = GetPropertyHeaderAddress(args[0]);
            string s = _zsciiString.GetZsciiString((ushort)(addr + 1));
            _io.Print(s);
            Log.Write($"[{s}]");
        }

        private void PrintAddr(List<ushort> args)
        {
            string s = _zsciiString.GetZsciiString(args[0]);
            _io.Print(s);
            Log.Write($"[{s}]");
        }

        private void PrintPAddr(List<ushort> args)
        {
            string s = _zsciiString.GetZsciiString(GetPackedAddress(args[0]));
            _io.Print(s);
            Log.Write($"[{s}]");
        }

        private void ShowStatus(List<ushort> args)
        {
            _io.ShowStatus();
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

            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, 0);
        }

        private void SetColor(List<ushort> args)
        {
            _io.SetColor((ZColor) args[0], (ZColor) args[1]);
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

            if (_terminateOnInput)
                _running = false;
            else
            {
                byte max = Memory[ReadTextAddr];
                string input = _io.Read(max);
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

                int ix = 1;

                if (Version >= 5)
                    Memory[ReadTextAddr + ix++] = (byte) input.Length;

                for (int j = 0; j < input.Length; j++, ix++)
                    Memory[ReadTextAddr + ix] = (byte) input[j];

                if (Version < 5)
                    Memory[ReadTextAddr + ++ix] = 0;

                string[] tokenised = input.Split(' ');

                Memory[ReadParseAddr + 1] = (byte) tokenised.Length;

                int len = (Version <= 3) ? 6 : 9;
                int last = 0;
                int max = Math.Min(tokenised.Length, wordMax);

                for (int i = 0; i < max; i++)
                {
                    if (tokenised[i].Length > len)
                        tokenised[i] = tokenised[i].Substring(0, len);

                    ushort wordIndex = (ushort) (Array.IndexOf(_dictionaryWords, tokenised[i]));
                    ushort addr = (ushort) (wordIndex == 0xffff ? 0 : _wordStart + wordIndex * _entryLength);
                    StoreWord((ushort) (ReadParseAddr + 2 + i * 4), addr);
                    Memory[ReadParseAddr + 4 + i * 4] = (byte) tokenised[i].Length;
                    int index = input.IndexOf(tokenised[i], last, StringComparison.Ordinal);
                    Memory[ReadParseAddr + 5 + i * 4] = (byte) (index + (Version < 5 ? 1 : 2));
                    last = index + tokenised[i].Length;
                }

                if (Version >= 5)
                {
                    byte dest = Memory[Stack.Peek().PC++];
                    StoreByteInVariable(dest, 10);
                }

                ReadTextAddr = 0;
                ReadParseAddr = 0;
            }
        }

        private void ReadChar(List<ushort> args)
        {
            char key = _io.ReadChar();

            byte dest = Memory[Stack.Peek().PC++];
            StoreByteInVariable(dest, (byte) key);
        }

        private void InsertObj(List<ushort> args)
        {
            if (args[0] == 0 || args[1] == 0)
                return;

            Log.Write($"[{GetObjectName(args[0])}] [{GetObjectName(args[1])}] ");

            ushort obj1 = args[0];
            ushort obj2 = args[1];

            ushort obj1Addr = GetObjectAddress(args[0]);
            ushort obj2Addr = GetObjectAddress(args[1]);

            ushort parent1 = GetObjectNumber((ushort) (obj1Addr + _versionOffsets.Parent));
            ushort sibling1 = GetObjectNumber((ushort) (obj1Addr + _versionOffsets.Sibling));
            ushort child2 = GetObjectNumber((ushort) (obj2Addr + _versionOffsets.Child));

            ushort parent1Addr = GetObjectAddress(parent1);

            ushort parent1Child = GetObjectNumber((ushort) (parent1Addr + _versionOffsets.Child));
            ushort parent1ChildAddr = GetObjectAddress(parent1Child);
            ushort parent1ChildSibling = GetObjectNumber((ushort) (parent1ChildAddr + _versionOffsets.Sibling));

            if (parent1 == obj2 && child2 == obj1)
                return;

            // if parent1's child is obj1 we need to assign the sibling
            if (parent1Child == obj1)
            {
                // set parent1's child to obj1's sibling
                SetObjectNumber((ushort) (parent1Addr + _versionOffsets.Child), sibling1);
            }
            else // else if I'm not the child but there is a child, we need to link the broken sibling chain
            {
                ushort addr = parent1ChildAddr;
                ushort currentSibling = parent1ChildSibling;

                // while sibling of parent1's child has siblings
                while (currentSibling != 0)
                {
                    // if obj1 is the sibling of the current object
                    if (currentSibling == obj1)
                    {
                        // set the current object's sibling to the next sibling
                        SetObjectNumber((ushort) (addr + _versionOffsets.Sibling), sibling1);
                        break;
                    }

                    addr = GetObjectAddress(currentSibling);
                    currentSibling = GetObjectNumber((ushort) (addr + _versionOffsets.Sibling));
                }
            }

            // set obj1's parent to obj2
            SetObjectNumber((ushort) (obj1Addr + _versionOffsets.Parent), obj2);

            // set obj2's child to obj1
            SetObjectNumber((ushort) (obj2Addr + _versionOffsets.Child), obj1);

            // set obj1's sibling to obj2's child
            SetObjectNumber((ushort) (obj1Addr + _versionOffsets.Sibling), child2);
        }

        private void RemoveObj(List<ushort> args)
        {
            if (args[0] == 0)
                return;

            Log.Write($"[{GetObjectName(args[0])}] ");
            ushort objAddr = GetObjectAddress(args[0]);
            ushort parent = GetObjectNumber((ushort) (objAddr + _versionOffsets.Parent));
            ushort parentAddr = GetObjectAddress(parent);
            ushort parentChild = GetObjectNumber((ushort) (parentAddr + _versionOffsets.Child));
            ushort sibling = GetObjectNumber((ushort) (objAddr + _versionOffsets.Sibling));

            // if object is the first child, set first child to the sibling
            if (parent == args[0])
                SetObjectNumber((ushort) (parentAddr + _versionOffsets.Child), sibling);
            else if (parentChild != 0)
            {
                ushort addr = GetObjectAddress(parentChild);
                ushort currentSibling = GetObjectNumber((ushort) (addr + _versionOffsets.Sibling));

                // while sibling of parent1's child has siblings
                while (currentSibling != 0)
                {
                    // if obj1 is the sibling of the current object
                    if (currentSibling == args[0])
                    {
                        // set the current object's sibling to the next sibling
                        SetObjectNumber((ushort) (addr + _versionOffsets.Sibling), sibling);
                        break;
                    }

                    addr = GetObjectAddress(currentSibling);
                    currentSibling = GetObjectNumber((ushort) (addr + _versionOffsets.Sibling));
                }
            }

            // set the object's parent to nothing
            SetObjectNumber((ushort) (objAddr + _versionOffsets.Parent), 0);
        }

        private void GetProp(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            byte dest = Memory[Stack.Peek().PC++];
            ushort val = 0;

            ushort addr = GetPropertyAddress(args[0], (byte) args[1]);
            if (addr > 0)
            {
                byte propInfo = Memory[addr++];
                byte len;

                if (Version > 3 && (propInfo & 0x80) == 0x80)
                    len = (byte) (Memory[addr++] & 0x3f);
                else
                    len = (byte) ((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

                for (int i = 0; i < len; i++)
                    val |= (ushort) (Memory[addr + i] << (len - 1 - i) * 8);
            }
            else
                val = GetWord((ushort) (_objectTable + (args[1] - 1) * 2));

            StoreWordInVariable(dest, val);
        }

        private void GetPropAddr(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            byte dest = Memory[Stack.Peek().PC++];
            ushort addr = GetPropertyAddress(args[0], (byte) args[1]);

            if (addr > 0)
            {
                byte propInfo = Memory[addr + 1];

                if (Version > 3 && (propInfo & 0x80) == 0x80)
                    addr += 2;
                else
                    addr += 1;
            }

            StoreWordInVariable(dest, addr);
        }

        private void GetNextProp(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            bool next = false;

            byte dest = Memory[Stack.Peek().PC++];
            if (args[1] == 0)
                next = true;

            ushort propHeaderAddr = GetPropertyHeaderAddress(args[0]);
            byte size = Memory[propHeaderAddr];
            propHeaderAddr += (ushort) (size * 2 + 1);

            while (Memory[propHeaderAddr] != 0x00)
            {
                byte propInfo = Memory[propHeaderAddr];
                byte len;
                if (Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte) (Memory[++propHeaderAddr] & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte) ((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

                byte propNum = (byte) (propInfo & (Version <= 3 ? 0x1f : 0x3f));

                if (next)
                {
                    StoreByteInVariable(dest, propNum);
                    return;
                }

                if (propNum == args[1])
                    next = true;

                propHeaderAddr += (ushort) (len + 1);
            }

            StoreByteInVariable(dest, 0);
        }

        private void GetPropLen(List<ushort> args)
        {
            byte dest = Memory[Stack.Peek().PC++];
            byte propInfo = Memory[args[0] - 1];
            byte len;
            if (Version > 3 && (propInfo & 0x80) == 0x80)
            {
                len = (byte) (Memory[args[0] - 1] & 0x3f);
                if (len == 0)
                    len = 64;
            }
            else
                len = (byte) ((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

            StoreByteInVariable(dest, len);
        }

        private void PutProp(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            ushort prop = GetPropertyHeaderAddress(args[0]);
            byte size = Memory[prop];
            prop += (ushort) (size * 2 + 1);

            while (Memory[prop] != 0x00)
            {
                byte propInfo = Memory[prop++];
                byte len;
                if (Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte) (Memory[prop++] & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte) ((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

                byte propNum = (byte) (propInfo & (Version <= 3 ? 0x1f : 0x3f));
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

        private void TestAttribute(List<ushort> args)
        {
            var obj = args[0];
            var attr = args[1];

            Log.Write($"[{GetObjectName(obj)}] ");
            PrintObjectInfo(obj, false);

            ushort objectAddr = GetObjectAddress(obj);
            ulong attributes;
            ulong flag;

            if (Version <= 3)
            {
                attributes = GetUint(objectAddr);
                flag = 0x80000000 >> attr;
            }
            else
            {
                attributes = (ulong) GetUint(objectAddr) << 16 | GetWord((uint) (objectAddr + 4));
                flag = (ulong) (0x800000000000 >> attr);
            }

            bool branch = (flag & attributes) == flag;
            Jump(branch);
        }

        private void SetAttribute(List<ushort> args)
        {
            var obj = args[0];
            var attr = args[1];

            if (obj == 0)
                return;

            Log.Write($"[{GetObjectName(obj)}] ");

            ushort objectAddr = GetObjectAddress(obj);
            ulong attributes;
            ulong flag;

            if (Version <= 3)
            {
                attributes = GetUint(objectAddr);
                flag = 0x80000000 >> attr;
                attributes |= flag;
                StoreUint(objectAddr, (uint) attributes);
            }
            else
            {
                attributes = (ulong) GetUint(objectAddr) << 16 | GetWord((uint) (objectAddr + 4));
                flag = (ulong) (0x800000000000 >> attr);
                attributes |= flag;
                StoreUint(objectAddr, (uint) (attributes >> 16));
                StoreWord((ushort) (objectAddr + 4), (ushort) attributes);
            }
        }

        private void ClearAttribute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            ushort objectAddr = GetObjectAddress(args[0]);
            ulong attributes;
            ulong flag;

            if (Version <= 3)
            {
                attributes = GetUint(objectAddr);
                flag = 0x80000000 >> args[1];
                attributes &= ~flag;
                StoreUint(objectAddr, (uint) attributes);
            }
            else
            {
                attributes = (ulong) GetUint(objectAddr) << 16 | GetWord((uint) (objectAddr + 4));
                flag = (ulong) (0x800000000000 >> args[1]);
                attributes &= ~flag;
                StoreUint(objectAddr, (uint) attributes >> 16);
                StoreWord((ushort) (objectAddr + 4), (ushort) attributes);
            }
        }

        private void GetParent(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            ushort addr = GetObjectAddress(args[0]);
            ushort parent = GetObjectNumber((ushort) (addr + _versionOffsets.Parent));

            Log.Write($"[{GetObjectName(parent)}] ");

            byte dest = Memory[Stack.Peek().PC++];

            if (Version <= 3)
                StoreByteInVariable(dest, (byte) parent);
            else
                StoreWordInVariable(dest, parent);
        }

        private void GetChild(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            ushort addr = GetObjectAddress(args[0]);
            ushort child = GetObjectNumber((ushort) (addr + _versionOffsets.Child));

            Log.Write($"[{GetObjectName(child)}] ");

            byte dest = Memory[Stack.Peek().PC++];

            if (Version <= 3)
                StoreByteInVariable(dest, (byte) child);
            else
                StoreWordInVariable(dest, child);

            Jump(child != 0);
        }

        private void GetSibling(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            ushort addr = GetObjectAddress(args[0]);
            ushort sibling = GetObjectNumber((ushort) (addr + _versionOffsets.Sibling));

            Log.Write($"[{GetObjectName(sibling)}] ");

            byte dest = Memory[Stack.Peek().PC++];

            if (Version <= 3)
                StoreByteInVariable(dest, (byte) sibling);
            else
                StoreWordInVariable(dest, sibling);

            Jump(sibling != 0);
        }

        private void Load(List<ushort> args)
        {
            byte dest = Memory[Stack.Peek().PC++];
            ushort val = GetVariable((byte) args[0], false);
            StoreByteInVariable(dest, (byte) val);
        }

        private void Store(List<ushort> args)
        {
            StoreWordInVariable((byte) args[0], args[1], false);
        }

        private void StoreB(List<ushort> args)
        {
            ushort addr = (ushort) (args[0] + args[1]);
            Memory[addr] = (byte) args[2];
        }

        private void StoreW(List<ushort> args)
        {
            ushort addr = (ushort) (args[0] + 2 * args[1]);
            StoreWord(addr, args[2]);
        }

        private void LoadB(List<ushort> args)
        {
            ushort addr = (ushort) (args[0] + args[1]);
            byte b = Memory[addr];
            byte dest = Memory[Stack.Peek().PC++];
            StoreByteInVariable(dest, b);
        }

        private void LoadW(List<ushort> args)
        {
            ushort addr = (ushort) (args[0] + 2 * args[1]);
            ushort word = GetWord(addr);
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, word);
        }

        private void Jump(List<ushort> args)
        {
            Stack.Peek().PC = (uint) (Stack.Peek().PC + (short) (args[0] - 2));
            Log.Write($"-> {Stack.Peek().PC:X5}");
        }

        private void Je(List<ushort> args)
        {
            bool equal = false;
            for (int i = 1; i < args.Count; i++)
            {
                if (args[0] == args[i])
                {
                    equal = true;
                    break;
                }
            }

            Jump(equal);
        }

        private void Jz(List<ushort> args)
        {
            Jump(args[0] == 0);
        }

        private void Jl(List<ushort> args)
        {
            Jump((short) args[0] < (short) args[1]);
        }

        private void Jg(List<ushort> args)
        {
            Jump((short) args[0] > (short) args[1]);
        }

        private void Jin(List<ushort> args)
        {
            Log.Write($"C[{GetObjectName(args[0])}] P[{GetObjectName(args[1])}] ");

            ushort addr = GetObjectAddress(args[0]);
            ushort parent = GetObjectNumber((ushort) (addr + _versionOffsets.Parent));
            Jump(parent == args[1]);
        }

        private void Jump(bool flag)
        {
            bool branch;

            byte offset = Memory[Stack.Peek().PC++];
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

            bool executeBranch = branch && flag || !branch && !flag;

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
                byte offset2 = Memory[Stack.Peek().PC++];
                ushort final = (ushort) ((offset & 0x3f) << 8 | offset2);

                // this is a 14-bit number, so set the sign bit properly because we can jump backwards
                if ((final & 0x2000) == 0x2000)
                    final |= 0xc000;

                newOffset = (short) (final - 2);
            }

            if (executeBranch)
                Stack.Peek().PC += (uint) newOffset;

            Log.Write($"-> {Stack.Peek().PC:X5}");
        }

        private void Add(List<ushort> args)
        {
            short val = (short) (args[0] + args[1]);
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort) val);
        }

        private void Sub(List<ushort> args)
        {
            short val = (short) (args[0] - args[1]);
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort) val);
        }

        private void Mul(List<ushort> args)
        {
            short val = (short) (args[0] * args[1]);
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort) val);
        }

        private void Div(List<ushort> args)
        {
            byte dest = Memory[Stack.Peek().PC++];

            if (args[1] == 0)
                return;

            short val = (short) ((short) args[0] / (short) args[1]);
            StoreWordInVariable(dest, (ushort) val);
        }

        private void Mod(List<ushort> args)
        {
            short val = (short) ((short) args[0] % (short) args[1]);
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort) val);
        }

        private void Inc(List<ushort> args)
        {
            short val = (short) (GetVariable((byte) args[0]) + 1);
            StoreWordInVariable((byte) args[0], (ushort) val);
        }

        private void Dec(List<ushort> args)
        {
            short val = (short) (GetVariable((byte) args[0]) - 1);
            StoreWordInVariable((byte) args[0], (ushort) val);
        }

        private void ArtShift(List<ushort> args)
        {
            // keep the sign bit, so make it a short
            short val = (short) args[0];
            if ((short) args[1] > 0)
                val <<= args[1];
            else if ((short) args[1] < 0)
                val >>= -args[1];

            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort) val);
        }

        private void LogShift(List<ushort> args)
        {
            // kill the sign bit, so make it a ushort
            ushort val = args[0];
            if ((short) args[1] > 0)
                val <<= args[1];
            else if ((short) args[1] < 0)
                val >>= -args[1];

            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort) val);
        }

        private void Random(List<ushort> args)
        {
            ushort val = 0;

            if ((short) args[0] <= 0)
                _random = new Random(-args[0]);
            else
                val = (ushort) (_random.Next(0, args[0]) + 1);

            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, val);
        }

        private void Or(List<ushort> args)
        {
            ushort or = (ushort) (args[0] | args[1]);
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, or);
        }

        private void And(List<ushort> args)
        {
            ushort and = (ushort) (args[0] & args[1]);
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, and);
        }

        private void Not(List<ushort> args)
        {
            byte dest = Memory[Stack.Peek().PC++];
            StoreWordInVariable(dest, (ushort) ~args[0]);
        }

        private void Test(List<ushort> args)
        {
            Jump((args[0] & args[1]) == args[1]);
        }

        private void DecCheck(List<ushort> args)
        {
            short val = (short) GetVariable((byte) args[0]);
            val--;
            StoreWordInVariable((byte) args[0], (ushort) val);
            Jump(val < (short) args[1]);
        }

        private void IncCheck(List<ushort> args)
        {
            short val = (short) GetVariable((byte) args[0]);
            val++;
            StoreWordInVariable((byte) args[0], (ushort) val);
            Jump(val > (short) args[1]);
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
                    byte dest = Memory[Stack.Peek().PC++];
                    StoreWordInVariable(dest, 0);
                }

                return;
            }

            uint pc = GetPackedAddress(args[0]);
            Log.Write($"New PC: {pc:X5}");

            ZStackFrame zsf = new ZStackFrame {PC = pc, StoreResult = storeResult};
            Stack.Push(zsf);

            byte count = Memory[Stack.Peek().PC++];

            if (Version <= 4)
            {
                for (int i = 0; i < count; i++)
                {
                    zsf.Variables[i] = GetWord(Stack.Peek().PC);
                    Stack.Peek().PC += 2;
                }
            }

            for (int i = 0; i < args.Count - 1; i++)
                zsf.Variables[i] = args[i + 1];

            zsf.ArgumentCount = args.Count - 1;
        }

        private void Call1N(List<ushort> args)
        {
            Call(args, false);
        }

        private void Call1S(List<ushort> args)
        {
            Call(args, true);
        }

        private void Call2S(List<ushort> args)
        {
            Call(args, true);
        }

        private void Call2N(List<ushort> args)
        {
            Call(args, false);
        }

        private void CallVN(List<ushort> args)
        {
            Call(args, false);
        }

        private void CallVN2(List<ushort> args)
        {
            Call(args, false);
        }

        private void CallVS2(List<ushort> args)
        {
            Call(args, true);
        }

        private void Ret(List<ushort> args)
        {
            ZStackFrame sf = Stack.Pop();
            if (sf.StoreResult)
            {
                byte dest = Memory[Stack.Peek().PC++];
                StoreWordInVariable(dest, args[0]);
            }
        }

        private void RetPopped(List<ushort> args)
        {
            ushort val = Stack.Peek().RoutineStack.Pop();
            ZStackFrame sf = Stack.Pop();
            if (sf.StoreResult)
            {
                byte dest = Memory[Stack.Peek().PC++];
                StoreWordInVariable(dest, val);
            }
        }

        private void Pop(List<ushort> args)
        {
            if (Stack.Peek().RoutineStack.Count > 0)
                Stack.Peek().RoutineStack.Pop();
            else
                Stack.Pop();
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
            ushort val = Stack.Peek().RoutineStack.Pop();
            StoreWordInVariable((byte) args[0], val, false);
        }

        private List<ushort> GetOperands(byte opcode)
        {
            List<ushort> args = new List<ushort>();
            ushort arg;

            // Variable
            if ((opcode & 0xc0) == 0xc0)
            {
                byte types = Memory[Stack.Peek().PC++];
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
                byte type = (byte) (opcode >> 4 & 0x03);
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
            for (int i = 6; i >= 0; i -= 2)
            {
                byte type = (byte) ((types >> i) & 0x03);

                // omitted
                if (type == 0x03)
                    break;

                ushort arg = GetOperand((OperandType) type);
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
                    byte b = Memory[Stack.Peek().PC++];
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

        private uint GetUint(uint address)
        {
            return (uint) (Memory[address] << 24 | Memory[address + 1] << 16 | Memory[address + 2] << 8 |
                           Memory[address + 3]);
        }

        private void StoreUint(uint address, uint val)
        {
            Memory[address + 0] = (byte) (val >> 24);
            Memory[address + 1] = (byte) (val >> 16);
            Memory[address + 2] = (byte) (val >> 8);
            Memory[address + 3] = (byte) (val >> 0);
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
            ushort objectAddr = (ushort) (_objectTable + _versionOffsets.PropertyDefaultTableSize + (obj - 1) * _versionOffsets.ObjectSize);
            return objectAddr;
        }

        private ushort GetObjectNumber(ushort objectAddr)
        {
            if (Version <= 3)
                return Memory[objectAddr];
            return GetWord(objectAddr);
        }

        private void SetObjectNumber(ushort objectAddr, ushort obj)
        {
            if (Version <= 3)
                Memory[objectAddr] = (byte) obj;
            else
                StoreWord(objectAddr, obj);
        }

        private ushort GetPropertyHeaderAddress(ushort obj)
        {
            ushort objectAddr = GetObjectAddress(obj);
            ushort propAddr = (ushort) (objectAddr + _versionOffsets.Property);
            ushort prop = GetWord(propAddr);
            return prop;
        }

        private ushort GetPropertyAddress(ushort obj, byte prop)
        {
            ushort propHeaderAddr = GetPropertyHeaderAddress(obj);

            // skip past text
            byte size = Memory[propHeaderAddr];
            propHeaderAddr += (ushort) (size * 2 + 1);

            while (Memory[propHeaderAddr] != 0x00)
            {
                byte propInfo = Memory[propHeaderAddr];
                byte propNum = (byte) (propInfo & (Version <= 3 ? 0x1f : 0x3f));

                if (propNum == prop)
                    return propHeaderAddr;

                byte len;

                if (Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte) (Memory[++propHeaderAddr] & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte) ((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

                propHeaderAddr += (ushort) (len + 1);
            }

            return 0;
        }

        private string GetObjectName(ushort obj)
        {
            string s = string.Empty;

            if (obj != 0)
            {
                ushort addr = GetPropertyHeaderAddress(obj);
                if (Memory[addr] != 0)
                {
                    s = _zsciiString.GetZsciiString((uint)(addr + 1));
                }
            }

            return s;
        }

        private void PrintObjects()
        {
            ushort lowest = 0xffff;

            for (ushort i = 1; i < 255 && (_objectTable + i * _versionOffsets.ObjectSize) < lowest; i++)
            {
                ushort addr = PrintObjectInfo(i, true);
                if (addr < lowest)
                    lowest = addr;
            }
        }

        private void PrintObjectTree()
        {
            for (ushort i = 1; i < 255; i++)
            {
                ushort addr = GetObjectAddress(i);
                ushort parent = GetObjectNumber((ushort) (addr + _versionOffsets.Parent));
                if (parent == 0)
                    PrintTree(i, 0);
            }
        }

        private void PrintTree(ushort obj, int depth)
        {
            while (obj != 0)
            {
                for (int i = 0; i < depth; i++)
                    Log.Write(" . ");

                PrintObjectInfo(obj, false);
                ushort addr = GetObjectAddress(obj);
                ushort child = GetObjectNumber((ushort) (addr + _versionOffsets.Child));
                obj = GetObjectNumber((ushort) (addr + _versionOffsets.Sibling));
                if (child != 0)
                    PrintTree(child, depth + 1);
            }
        }

        private ushort PrintObjectInfo(ushort obj, bool properties)
        {
            if (obj == 0)
                return 0;

            ushort startAddr = GetObjectAddress(obj);

            ulong attributes = (ulong) GetUint(startAddr) << 16 | GetWord((uint) (startAddr + 4));
            ushort parent = GetObjectNumber((ushort) (startAddr + _versionOffsets.Parent));
            ushort sibling = GetObjectNumber((ushort) (startAddr + _versionOffsets.Sibling));
            ushort child = GetObjectNumber((ushort) (startAddr + _versionOffsets.Child));
            ushort propAddr = GetWord((uint) (startAddr + _versionOffsets.Property));

            Log.Write($"{obj} ({obj:X2}) at {propAddr:X5}: ");

            byte size = Memory[propAddr++];
            string s = string.Empty;
            if (size > 0)
            {
                s = _zsciiString.GetZsciiString(propAddr);
            }

            propAddr += (ushort) (size * 2);

            Log.WriteLine(
                $"[{s}] A:{attributes:X12} P:{parent}({parent:X2}) S:{sibling}({sibling:X2}) C:{child}({child:X2})");

            if (properties)
            {
                string ss = string.Empty;
                for (int i = 47; i >= 0; i--)
                {
                    if (((attributes >> i) & 0x01) == 0x01)
                    {
                        ss += 47 - i + ", ";
                    }
                }

                Log.WriteLine("Attributes: " + ss);

                while (Memory[propAddr] != 0x00)
                {
                    byte propInfo = Memory[propAddr];
                    byte len;
                    if (Version > 3 && (propInfo & 0x80) == 0x80)
                        len = (byte) (Memory[propAddr + 1] & 0x3f);
                    else
                        len = (byte) ((propInfo >> (Version <= 3 ? 5 : 6)) + 1);
                    byte propNum = (byte) (propInfo & (Version <= 3 ? 0x1f : 0x3f));

                    Log.Write($"  P:{propNum:X2} at {propAddr:X4}: ");
                    for (int i = 0; i < len; i++)
                        Log.Write($"{Memory[propAddr++]:X2} ");
                    Log.WriteLine("");
                    propAddr++;
                }
            }

            return propAddr;
        }

        private void ParseDictionary()
        {
            ushort address = _dictionary;

            byte len = Memory[address++];
            address += len;

            _entryLength = Memory[address++];
            ushort numEntries = GetWord(address);
            address += 2;

            _wordStart = address;

            _dictionaryWords = new string[numEntries];

            for (int i = 0; i < numEntries; i++)
            {
                ushort wordAddress = (ushort) (address + i * _entryLength);
                var chars = _zsciiString.GetZsciiChar(wordAddress);
                chars.AddRange(_zsciiString.GetZsciiChar((uint) (wordAddress + 2)));
                if (_entryLength == 9)
                    chars.AddRange(_zsciiString.GetZsciiChar((uint) (wordAddress + 4)));
                string s = _zsciiString.DecodeZsciiChars(chars);
                _dictionaryWords[i] = s;
            }
        }

        private void Restore(List<ushort> args) => _kind0Ops.Restore.Execute(args);
        private void Save(List<ushort> args) => _kind0Ops.Save.Execute(args);
    }
}