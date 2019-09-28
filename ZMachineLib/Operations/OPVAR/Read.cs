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
            Contents.ReadTextAddr = args[0];
            Contents.ReadParseAddr = args[1];

            if (Contents.TerminateOnInput)
                Contents.Running = false;
            else
            {
                var max = Contents.Manager.Get(Contents.ReadTextAddr);
                var input = _io.Read(max);
                FinishRead(input);
            }
        }

        private void FinishRead(string input)
        {
            if (input != null && Contents.ReadTextAddr != 0 && Contents.ReadParseAddr != 0)
            {
                int textMax = Contents.Manager.Get(Contents.ReadTextAddr);
                int wordMax = Contents.Manager.Get(Contents.ReadParseAddr);

                input = input.ToLower().Substring(0, Math.Min(input.Length, textMax));
                Log.Write($"[{input}]");

                var ix = 1;

                if (Contents.Header.Version >= 5)
                    Contents.Manager.Set(Contents.ReadTextAddr + ix++, (byte)input.Length);

                for (var j = 0; j < input.Length; j++, ix++)
                    Contents.Manager.Set(Contents.ReadTextAddr + ix, (byte)input[j]);

                if (Contents.Header.Version < 5)
                    Contents.Manager.Set(Contents.ReadTextAddr + ++ix, 0);

                var tokenised = input.Split(' ');

                Contents.Manager.Set(Contents.ReadParseAddr + 1,  (byte)tokenised.Length);

                var len = ((ushort)Contents.Header.Version <= 3) ? 6 : 9;
                var last = 0;
                var max = Math.Min(tokenised.Length, wordMax);

                for (var i = 0; i < max; i++)
                {
                    if (tokenised[i].Length > len)
                        tokenised[i] = tokenised[i].Substring(0, len);

                    var wordIndex = (ushort)(Array.IndexOf(Contents.Dictionary.Words, tokenised[i]));
                    var addr = (ushort)(wordIndex == 0xffff ? 0 : Contents.DictionaryWordStart + wordIndex * Contents.Dictionary.EntryLength);
                    Contents.Manager.Set((ushort)(Contents.ReadParseAddr + 2 + i * 4), addr);
                    Contents.Manager.Set(Contents.ReadParseAddr + 4 + i * 4, (byte)tokenised[i].Length);
                    var index = input.IndexOf(tokenised[i], last, StringComparison.Ordinal);
                    Contents.Manager.Set(Contents.ReadParseAddr + 5 + i * 4, (byte)(index + ((ushort)Contents.Header.Version < 5 ? 1 : 2)));
                    last = index + tokenised[i].Length;
                }

                if (Contents.Header.Version >= 5)
                {
                    var dest = Contents.GetCurrentByteAndInc();
                    Contents.VariableManager.Store(dest, 10);
                }

                Contents.ReadTextAddr = 0;
                Contents.ReadParseAddr = 0;
            }
        }

    }
}