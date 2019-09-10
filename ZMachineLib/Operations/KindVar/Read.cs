using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class Read : ZMachineOperation
    {
        private readonly IZMachineIo _io;

        public Read(ZMachine2 machine, IZMachineIo io)
            : base((ushort)KindVarOpCodes.Read, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            ReadTextAddr = args[0];
            ReadParseAddr = args[1];

            if (Machine.TerminateOnInput)
                Machine._running = false;
            else
            {
                var max = Memory[ReadTextAddr];
                var input = _io.Read(max);
                FinishRead(input);
            }
        }

        private void FinishRead(string input)
        {
            if (input != null && ReadTextAddr != 0 && ReadParseAddr != 0)
            {
                int textMax = Memory[ReadTextAddr];
                int wordMax = Memory[ReadParseAddr];

                input = input.ToLower().Substring(0, Math.Min(input.Length, textMax));
                Log.Write($"[{input}]");

                var ix = 1;

                if (Version >= 5)
                    Memory[ReadTextAddr + ix++] = (byte)input.Length;

                for (var j = 0; j < input.Length; j++, ix++)
                    Memory[ReadTextAddr + ix] = (byte)input[j];

                if (Version < 5)
                    Memory[ReadTextAddr + ++ix] = 0;

                var tokenised = input.Split(' ');

                Memory[ReadParseAddr + 1] = (byte)tokenised.Length;

                var len = (Version <= 3) ? 6 : 9;
                var last = 0;
                var max = Math.Min(tokenised.Length, wordMax);

                for (var i = 0; i < max; i++)
                {
                    if (tokenised[i].Length > len)
                        tokenised[i] = tokenised[i].Substring(0, len);

                    var wordIndex = (ushort)(Array.IndexOf(Machine._dictionaryWords, tokenised[i]));
                    var addr = (ushort)(wordIndex == 0xffff ? 0 : Machine._wordStart + wordIndex * Machine._entryLength);
                    StoreWord((ushort)(ReadParseAddr + 2 + i * 4), addr);
                    Memory[ReadParseAddr + 4 + i * 4] = (byte)tokenised[i].Length;
                    var index = input.IndexOf(tokenised[i], last, StringComparison.Ordinal);
                    Memory[ReadParseAddr + 5 + i * 4] = (byte)(index + (Version < 5 ? 1 : 2));
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

    }
}