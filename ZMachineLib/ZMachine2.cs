using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using ZMachineLib.Operations;
using ZMachineLib.Content;
using ZMachineLib.Operations.OPExtended;

namespace ZMachineLib
{
    public class ZMachine2
    {
        internal byte[] _memory;
        internal readonly Stack<ZStackFrame> Stack = new Stack<ZStackFrame>();
        public IZMemory Memory { get; private set; }
        
        internal bool Running;

        private Stream _gameFileStream;

        private readonly IUserIo _io;
        private readonly IFileIo _fileIo;
        private KindExtOperations _extendedOperations;
        private Operations.Operations _operations;
        private byte[] _restartState;

        public ZMachine2(IUserIo io, IFileIo fileIo)
        {
            _fileIo = fileIo;
            _io = io;
        }

        public void RunFile(Stream stream, bool terminateOnInput = false)
        {
            _gameFileStream = stream;
            bool restart = true;
            while (restart)
            {
                LoadFile(stream);
                restart = Run();
            };
        }

        public void RunFile(string filename)
        {
            var fileStream = File.OpenRead(filename);
            RunFile(fileStream);
        }

        public bool Run(bool terminateOnInput = false)
        {
            Memory.TerminateOnInput = terminateOnInput;

            Memory.Running = true;
            bool restart = false;
            while (Memory.Running)
            {
                Log.Write($"PC: {Stack.Peek().PC:X5}");
                var opCode = Memory.GetCurrentByteAndInc(); 
                IOperation operation;
                OpCodes opCodeEnum;
                (opCode, opCodeEnum, operation) = GetOperation(opCode);

                if (opCodeEnum == OpCodes.Restart)
                {
                    restart = true;
                    break;
                }
                if (operation == null) throw new Exception($"No operation found for Op Code {opCode} ({opCode:X2})!");

                Log.WriteLine($" OP: {opCodeEnum:D} ({(byte)opCodeEnum:X2}) - {operation.GetType().Name})");

                operation.Execute(Memory.OperandManager.GetOperands(opCode));

                Log.Flush();
            }

            return restart;
        }

        private (byte opCode, OpCodes opCodeEnum, IOperation operation) GetOperation(byte opCode)
        {
            //NOTE: http://inform-fiction.org/zmachine/standards/z1point1/sect14.html
            IOperation operation;
            OpCodes opCodeEnum;
            if (opCode == (byte)OpCodes.Extended) // 0OP:190 - special op, indicates next byte contains Extended Op
            {
                opCodeEnum = OpCodes.Extended;
                opCode = Memory.GetCurrentByteAndInc();
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
            _memory = Read(stream);
            _restartState = (byte[])_memory.Clone();
            Execute(_memory);
        }

        private void Execute(byte[] memory)
        {
            Memory = new ZMemory(memory, Stack, ReloadFile);

            SetupNewOperations();

            SetupScreenParams();

            var zsf = new ZStackFrame {PC = Memory.Header.Pc};
            Stack.Push(zsf);
        }

        private void SetupScreenParams()
        {
            // NOTE: These don't seem to do anything on a standard console window
            Memory.Manager.Set(0, 0x01);
            Memory.Manager.Set(0x20, _io.ScreenHeight);
            Memory.Manager.Set(0x21, _io.ScreenWidth);
        }


        private byte[] Read(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, (int) stream.Length);
            return buffer;
        }

        private void SetupNewOperations()
        {
            _operations = new Operations.Operations(this, _io, _fileIo);
            _extendedOperations = new KindExtOperations(this, _operations);
        }

        private void ReloadFile() => Execute(_restartState);
    }
}