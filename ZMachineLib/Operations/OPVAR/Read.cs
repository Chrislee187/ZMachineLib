﻿using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Read : ZMachineOperationBase
    {
        private readonly IUserIo _io;
        private readonly ZmDebugger _debugger = new ZmDebugger();
        private ushort _lastReadTextAddr;
        private ushort _lastReadParseAddr;

        public Read(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.Read, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            var max = Memory.Manager.Get(args[0]);
            bool isDebugCommand;
            string input;
            
            do
            {
                input = _io.Read(max, Memory);
                string debugOutput;
                (isDebugCommand, debugOutput) = _debugger.HandleDebugCommand(Memory, input);

                if (isDebugCommand)
                {
                    // TODO: Create seperate endpoint for Debug output
                    _io.Print(debugOutput);
                }

            } while (isDebugCommand);

            _lastReadTextAddr = args[0];
            _lastReadParseAddr = args[1];
            SetupParseTables(input, _lastReadTextAddr, _lastReadParseAddr);
        }

        public bool ReadContinue(string input)
        {
            if(_debugger == null) throw new Exception("NULLNULL");

            var (isDebugCommand, debugOutput) = _debugger.HandleDebugCommand(Memory, input);

            if (isDebugCommand)
            {
                // TODO: Create seperate endpoint for Debug output
                _io.Print(debugOutput);
                return false;
            }

            SetupParseTables(input, _lastReadTextAddr, _lastReadParseAddr);
            return true;
        }

        private void SetupParseTables(string input, ushort readTextAddr, ushort readParseAddr)
        {
            if (input != null && readTextAddr != 0 && readParseAddr != 0)
            {
                int textMax = Memory.Manager.Get(readTextAddr);
                int wordMax = Memory.Manager.Get(readParseAddr);

                input = input.ToLower().Substring(0, Math.Min(input.Length, textMax));
                Log.Write($"[{input}]");

                var ix = 1;

                if (Memory.Header.Version >= 5)
                    Memory.Manager.Set(readTextAddr + ix++, (byte)input.Length);

                for (var j = 0; j < input.Length; j++, ix++)
                    Memory.Manager.Set(readTextAddr + ix, (byte)input[j]);

                if (Memory.Header.Version < 5)
                    Memory.Manager.Set(readTextAddr + ++ix, 0);

                var tokenised = input.Split(' ');

                Memory.Manager.Set(readParseAddr + 1,  (byte)tokenised.Length);

                var len = ((ushort)Memory.Header.Version <= 3) ? 6 : 9;
                var last = 0;
                var max = Math.Min(tokenised.Length, wordMax);

                for (var i = 0; i < max; i++)
                {
                    if (tokenised[i].Length > len)
                        tokenised[i] = tokenised[i].Substring(0, len);

                    var wordIndex = (ushort)(Array.IndexOf(Memory.Dictionary.Words, tokenised[i]));
                    var addr = (ushort)(wordIndex == 0xffff ? 0 : Memory.DictionaryWordStart + wordIndex * Memory.Dictionary.EntryLength);
                    Memory.Manager.Set((ushort)(readParseAddr + 2 + i * 4), addr);
                    Memory.Manager.Set(readParseAddr + 4 + i * 4, (byte)tokenised[i].Length);
                    var index = input.IndexOf(tokenised[i], last, StringComparison.Ordinal);
                    Memory.Manager.Set(readParseAddr + 5 + i * 4, (byte)(index + ((ushort)Memory.Header.Version < 5 ? 1 : 2)));
                    last = index + tokenised[i].Length;
                }

                if (Memory.Header.Version >= 5)
                {
                    var dest = Memory.GetCurrentByteAndInc();
                    Memory.VariableManager.Store(dest, 10);
                }
            }

            SetParseAddresses(0,0);
        }

        public void SetParseAddresses(ushort lastReadTextAddr, ushort lastReadParseAddr)
        {
            _lastReadTextAddr = lastReadTextAddr;
            _lastReadParseAddr = lastReadParseAddr;
        }
    }
}