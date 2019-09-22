using System;
using System.Collections.Generic;
using System.IO;
using ZMachineLib.Operations;
using System.Text.Json;
using ZMachineLib.Content;
using ZMachineLib.Operations.OPExtended;

namespace ZMachineLib
{
    public class ZMachine2
    {
        internal byte[] Memory;
        internal Stack<ZStackFrame> Stack = new Stack<ZStackFrame>();


        public ZMachineContents Contents { get; set; }

        internal ushort ReadTextAddr;
        internal ushort ReadParseAddr;

        internal bool TerminateOnInput;
        internal bool Running;

        private Stream _gameFileStream;

        private readonly IUserIo _io;
        private readonly IFileIo _fileIo;
        private KindExtOperations _extendedOperations;
        private Operations.Operations _operations;
        public ZHeader Header => Contents.Header;
        internal VersionedOffsets VersionedOffsets;

        public ZMachine2(IUserIo io, IFileIo fileIo)
        {
            _fileIo = fileIo;
            _io = io;

            VersionedOffsets = new VersionedOffsets();
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

        public void Run(bool terminateOnInput = false)
        {
            TerminateOnInput = terminateOnInput;

            Running = true;

            while (Running)
            {
                Log.Write($"PC: {Stack.Peek().PC:X5}");
                var opCode = Memory[Stack.Peek().PC++];
                IOperation operation;
                OpCodes opCodeEnum;
                (opCode, opCodeEnum, operation) = GetOperation(opCode);

                if (operation == null) throw new Exception($"No operation found for Op Code {opCode} ({opCode:X2})!");

                Log.WriteLine($" OP: {opCodeEnum:D} ({(byte)opCodeEnum:X2}) - {operation.GetType().Name})");

                operation.Execute(Contents.OperandManager.GetOperands(opCode));

                Log.Flush();
            }
        }

        private (byte opCode, OpCodes opCodeEnum, IOperation operation) GetOperation(byte opCode)
        {
            //NOTE: http://inform-fiction.org/zmachine/standards/z1point1/sect14.html
            IOperation operation;
            OpCodes opCodeEnum;
            if (opCode == (byte)OpCodes.Extended) // 0OP:190 - special op, indicates next byte contains Extended Op
            {
                opCodeEnum = OpCodes.Extended;
                opCode = Memory[Stack.Peek().PC++];
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

        private void LoadFile(Stream stream)
        {
            Memory = Read(stream);
            Contents = new ZMachineContents(Memory, Stack);

            SetupNewOperations();
#if DEBUG
            DumpHeader();
#endif

            SetupScreenParams();

            VersionedOffsets = VersionedOffsets.For(Header.Version);

            var zsf = new ZStackFrame {PC = Header.Pc };
            Stack.Push(zsf);
        }

        private void SetupScreenParams()
        {
            // TODO: These should be part of the zHeader????
            Memory[0x01] = 0x01; // Sets Flags1 to Status Line = hours:mins
            Memory[0x20] = _io.ScreenHeight; 
            Memory[0x21] = _io.ScreenWidth;
        }

#if DEBUG
        private void DumpHeader()
        {
            var jsonSerializerOptions = new JsonSerializerOptions {WriteIndented = true};
            Console.WriteLine(JsonSerializer.Serialize(Header, jsonSerializerOptions));
        }
#endif

        private byte[] Read(Stream stream)
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
            _operations = new Operations.Operations(this, _io, _fileIo);
            RTrue = _operations[OpCodes.RTrue];
            RFalse = _operations[OpCodes.RFalse];
            _extendedOperations = new KindExtOperations(this, _operations);
        }

        internal void ReloadFile() => LoadFile(_gameFileStream);
    }
}