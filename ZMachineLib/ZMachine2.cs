using System;
using System.IO;
using Microsoft.Extensions.Logging;
using ZMachineLib.Operations;
using ZMachineLib.Content;
using ZMachineLib.Extensions;
using ZMachineLib.Operations.OPVAR;

namespace ZMachineLib
{
    public class ZMachine2
    {
        private IZMemory _zMemory;
        public bool Running => _zMemory.Running;

        private readonly IUserIo _io;
        private readonly IFileIo _fileIo;

        private byte[] _restartState;

        private ZOperations _zOperations;
        private readonly ILogger _logger;

        private bool _interruptMode = true;

        public ZMachine2(IUserIo io, IFileIo fileIo,
            ILogger logger = null)
        {

            _logger = logger;
            _fileIo = fileIo;
            _io = io;
        }

        /// <summary>
        /// The default, interrupt driven program execution loop. Program will run until
        /// it's exits, (i.e. when the user 'quits' the game) This method WILL NOT return
        /// until that point.
        /// <seealso cref="https://en.wikipedia.org/wiki/Interrupt"/>
        /// </summary>
        public void RunFile(Stream stream, bool interruptMode = true) => RunFileTillRead(stream, interruptMode);

        public void RunFile(string filename) => RunFile(File.OpenRead(filename));

        private void RunFileTillRead(Stream stream, bool interruptMode)
        {
            CheckIfMachineAlreadyRunning();
            _interruptMode = interruptMode;
            bool restart = true;
            while (restart)
            {
                LoadFile(stream);
                restart = Run();
            }
        }
        private bool Run()
        {
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

                if (opCodeEnum == OpCodes.Read && !_interruptMode)
                {
                    _io.ShowStatus(_zMemory);
                    Read read = (Read)operation;
                    read.SetParseAddresses(args[0], args[1]);
                    break;
                }

                operation.Execute(args);

                Log.Flush();
            }

            return restart;
        }
        
        private void CheckIfMachineAlreadyRunning()
        {
            if (_zMemory != null && _zMemory.Running)
            {
                throw new InvalidOperationException("Machine already running!");
            }
        }
        
        /// <summary>
        /// Partnered with RunFileTillRead(). This is where we supply the program
        /// with the next user input and continue execution till the next read.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void ContinueTillNextRead(string input)
        {
            if(_interruptMode) throw new InvalidOperationException($"Cannot use ContinueTillNextRead() when running in INTERRUPT mode, use RunFileTillRead() to execute programs in non-interrupt mode.");

            var (_, _, operation) 
                = _zOperations.GetOperation((byte) OpCodes.Read);

            var read = (Read) operation;

            if (!read.ReadContinue(input)) return;

            Run();
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