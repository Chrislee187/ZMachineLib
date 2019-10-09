using System;
using System.IO;
using Microsoft.Extensions.Logging;
using ZMachineLib.Operations;
using ZMachineLib.Content;
using ZMachineLib.Extensions;

namespace ZMachineLib
{
    public class ZMachine2
    {
        private IZMemory _zMemory;

        private readonly IUserIo _io;
        private readonly IFileIo _fileIo;

        private byte[] _restartState;

        private ZOperations _zOperations;
        private readonly ILogger _logger;

        public ZMachine2(IUserIo io, IFileIo fileIo,
            ILogger logger = null)
        {

            _logger = logger;
            _fileIo = fileIo;
            _io = io;
        }

        public void RunFile(Stream stream, bool terminateOnInput = false)
        {
            bool restart = true;
            while (restart)
            {
                LoadFile(stream);
                restart = Run(terminateOnInput);
            }
        }

        public void RunFile(string filename, bool terminateOnInput = false)
        {
            var fileStream = File.OpenRead(filename);
            RunFile(fileStream, terminateOnInput);
        }

        private bool Run(bool terminateOnInput = false)
        {
            _zMemory.TerminateOnInput = terminateOnInput;
            _zMemory.Running = true;
            bool restart = false;
            while (_zMemory.Running)
            {
                var opCode = _zMemory.GetCurrentByteAndInc(); 
                IOperation operation;
                OpCodes opCodeEnum;
                (opCode, opCodeEnum, operation) = _zOperations.GetOperation(opCode);

                if (opCodeEnum == OpCodes.Restart)
                {
                    restart = true;
                    break;
                }
                if (operation == null) throw new Exception($"No operation found for Op Code {opCode} ({opCode:X2})!");

                Log.WriteLine($" OP: {opCodeEnum:D} ({(byte)opCodeEnum:X2}) - {operation.GetType().Name})");

                var args = _zMemory.OperandManager.GetOperands(opCode);
                operation.Execute(args);

                Log.Flush();
            }

            return restart;
        }

        private void LoadFile(Stream stream)
        {

            byte[] memory = stream.ToByteArray();

            InitialiseMachine(memory);
        }

        private void InitialiseMachine(byte[] memory)
        {
            _logger.InfoMessage("Initialising ZMachine");
            _restartState = (byte[]) memory.Clone();
            _zMemory = new ZMemory(memory, () => InitialiseMachine(_restartState));
            _zOperations = new ZOperations(_io, _fileIo, _zMemory);

            _zMemory.Stack.Push(new ZStackFrame { PC = _zMemory.Header.Pc });
        }

    }
}