using System;
using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Read : ZMachineOperationBase
    {
        private readonly IUserIo _io;

        public Read(IZMemory memory, IUserIo io)
            : base((ushort)OpCodes.Read, memory)
        {
            _io = io;
        }

        public override void Execute(List<ushort> args)
        {
            if (Memory.TerminateOnInput)
                Memory.Running = false;
            else
            {
                var max = Memory.Manager.Get(args[0]);
                var input = _io.Read(max);
                FinishRead(input, args[0], args[1]);
            }
        }

        private void FinishRead(string input, ushort readTextAddr, ushort readParseAddr)
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
        }

    }
}