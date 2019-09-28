using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (Contents.TerminateOnInput)
                Contents.Running = false;
            else
            {
                var max = Contents.Manager.Get(args[0]);
                var input = _io.Read(max);
                FinishRead(input, args[0], args[1]);
            }
        }

        private void FinishRead(string input, ushort readTextAddr, ushort readParseAddr)
        {
            if (input != null && readTextAddr != 0 && readParseAddr != 0)
            {
                int textMax = Contents.Manager.Get(readTextAddr);
                int wordMax = Contents.Manager.Get(readParseAddr);

                input = input.ToLower().Substring(0, Math.Min(input.Length, textMax));
                Log.Write($"[{input}]");

                var ix = 1;

                if (Contents.Header.Version >= 5)
                    Contents.Manager.Set(readTextAddr + ix++, (byte)input.Length);

                for (var j = 0; j < input.Length; j++, ix++)
                    Contents.Manager.Set(readTextAddr + ix, (byte)input[j]);

                if (Contents.Header.Version < 5)
                    Contents.Manager.Set(readTextAddr + ++ix, 0);

                var tokenised = input.Split(' ');

                Contents.Manager.Set(readParseAddr + 1,  (byte)tokenised.Length);

                var len = ((ushort)Contents.Header.Version <= 3) ? 6 : 9;
                var last = 0;
                var max = Math.Min(tokenised.Length, wordMax);

                for (var i = 0; i < max; i++)
                {
                    if (tokenised[i].Length > len)
                        tokenised[i] = tokenised[i].Substring(0, len);

                    var wordIndex = (ushort)(Array.IndexOf(Contents.Dictionary.Words, tokenised[i]));
                    var addr = (ushort)(wordIndex == 0xffff ? 0 : Contents.DictionaryWordStart + wordIndex * Contents.Dictionary.EntryLength);
                    Contents.Manager.Set((ushort)(readParseAddr + 2 + i * 4), addr);
                    Contents.Manager.Set(readParseAddr + 4 + i * 4, (byte)tokenised[i].Length);
                    var index = input.IndexOf(tokenised[i], last, StringComparison.Ordinal);
                    Contents.Manager.Set(readParseAddr + 5 + i * 4, (byte)(index + ((ushort)Contents.Header.Version < 5 ? 1 : 2)));
                    last = index + tokenised[i].Length;
                }

                if (Contents.Header.Version >= 5)
                {
                    var dest = Contents.GetCurrentByteAndInc();
                    Contents.VariableManager.Store(dest, 10);
                }
            }
        }

    }
}