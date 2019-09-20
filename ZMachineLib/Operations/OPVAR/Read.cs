using System;
using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class Read : ZMachineOperation
    {
        private readonly IUserIo _io;

        public Read(ZMachine2 machine, IUserIo io)
            : base((ushort)OpCodes.Read, machine)
        {
            _io = io;
        }

        public override void Execute(List<ushort> operands)
        {
            Machine.ReadTextAddr = operands[0];
            Machine.ReadParseAddr = operands[1];

            if (Machine.TerminateOnInput)
                Machine.Running = false;
            else
            {
                var max = MemoryManager.Get(Machine.ReadTextAddr);
                var input = _io.Read(max);
                FinishRead(input);
            }
        }

        private void FinishRead(string input)
        {
            if (input != null && Machine.ReadTextAddr != 0 && Machine.ReadParseAddr != 0)
            {
                int textMax = MemoryManager.Get(Machine.ReadTextAddr);
                int wordMax = MemoryManager.Get(Machine.ReadParseAddr);

                input = input.ToLower().Substring(0, Math.Min(input.Length, textMax));
                Log.Write($"[{input}]");

                var ix = 1;

                if (Machine.Header.Version >= 5)
                    MemoryManager.Set(Machine.ReadTextAddr + ix++, (byte)input.Length);

                for (var j = 0; j < input.Length; j++, ix++)
                    MemoryManager.Set(Machine.ReadTextAddr + ix, (byte)input[j]);

                if (Machine.Header.Version < 5)
                    MemoryManager.Set(Machine.ReadTextAddr + ++ix, 0);

                var tokenised = input.Split(' ');

                MemoryManager.Set(Machine.ReadParseAddr + 1,  (byte)tokenised.Length);

                var len = ((ushort) Machine.Header.Version <= 3) ? 6 : 9;
                var last = 0;
                var max = Math.Min(tokenised.Length, wordMax);

                for (var i = 0; i < max; i++)
                {
                    if (tokenised[i].Length > len)
                        tokenised[i] = tokenised[i].Substring(0, len);

                    var wordIndex = (ushort)(Array.IndexOf(Machine.Dictionary.Words, tokenised[i]));
                    var addr = (ushort)(wordIndex == 0xffff ? 0 : Machine.WordStart + wordIndex * Machine.EntryLength);
                    MemoryManager.Set((ushort)(Machine.ReadParseAddr + 2 + i * 4), addr);
                    MemoryManager.Set(Machine.ReadParseAddr + 4 + i * 4, (byte)tokenised[i].Length);
                    var index = input.IndexOf(tokenised[i], last, StringComparison.Ordinal);
                    MemoryManager.Set(Machine.ReadParseAddr + 5 + i * 4, (byte)(index + ((ushort) Machine.Header.Version < 5 ? 1 : 2)));
                    last = index + tokenised[i].Length;
                }

                if (Machine.Header.Version >= 5)
                {
                    var dest = PeekNextByte();
                    VariableManager.StoreByte(dest, 10);
                }

                Machine.ReadTextAddr = 0;
                Machine.ReadParseAddr = 0;
            }
        }

    }
}