using System.Collections.Generic;

namespace ZMachineLib.Operations
{
    public abstract class ZMachineOperation : IOperation
    {
        public Kind0OpCodes Code { get; }
        
        protected const ushort ZFalse = 0;
        protected const ushort ZTrue = 1;

        protected readonly ZMachine2 Machine;

        protected ZMachineOperation(Kind0OpCodes code,
            ZMachine2 machine)
        {
            Code = code;
            Machine = machine;
        }
        public abstract void Execute(List<ushort> args);


        private protected void StoreWordInVariable(byte dest, ushort value, bool push = true)
        {
            if (dest == 0)
            {
                Log.Write($"-> SP ({value:X4}), ");
                if (!push)
                    Machine.Stack.Peek().RoutineStack.Pop();
                Machine.Stack.Peek().RoutineStack.Push(value);
            }
            else if (dest < 0x10)
            {
                Log.Write($"-> L{dest - 1:X2} ({value:X4}), ");
                Machine.Stack.Peek().Variables[dest - 1] = value;
            }
            else
            {
                Log.Write($"-> G{dest - 0x10:X2} ({value:X4}), ");
                StoreWord((ushort)(Machine.Globals + 2 * (dest - 0x10)), value);
            }
        }

        private void StoreWord(ushort address, ushort value)
        {
            Machine.Memory[address + 0] = (byte)(value >> 8);
            Machine.Memory[address + 1] = (byte)value;
        }

        protected void Jump(bool flag)
        {
            bool branch;

            var offset = Machine.Memory[Machine.Stack.Peek().PC++];
            short newOffset;

            if ((offset & 0x80) == 0x80)
            {
                Log.Write(" [TRUE] ");
                branch = true;
            }
            else
            {
                Log.Write(" [FALSE] ");
                branch = false;
            }

            var executeBranch = branch && flag || !branch && !flag;

            if ((offset & 0x40) == 0x40)
            {
                offset = (byte)(offset & 0x3f);

                if (offset == 0 && executeBranch)
                {
                    Log.Write(" RFALSE ");
                    Machine.RFalse.Execute(null);
                    return;
                }

                if (offset == 1 && executeBranch)
                {
                    Log.Write(" RTRUE ");
                    Machine.RTrue.Execute(null);
                    return;
                }

                newOffset = (short)(offset - 2);
            }
            else
            {
                var offset2 = Machine.Memory[Machine.Stack.Peek().PC++];
                var final = (ushort)((offset & 0x3f) << 8 | offset2);

                // this is a 14-bit number, so set the sign bit properly because we can jump backwards
                if ((final & 0x2000) == 0x2000)
                    final |= 0xc000;

                newOffset = (short)(final - 2);
            }

            if (executeBranch)
                Machine.Stack.Peek().PC += (uint)newOffset;

            Log.Write($"-> { Machine.Stack.Peek().PC:X5}");
        }


        protected List<byte> GetZsciiChars(uint address)
        {
            List<byte> chars = new List<byte>();
            ushort word;
            do
            {
                word = GetWord(address);
                chars.AddRange(GetZsciiChar(address));
                address += 2;
            }
            while ((word & 0x8000) != 0x8000);

            return chars;
        }

        protected List<byte> GetZsciiChar(uint address)
        {
            List<byte> chars = new List<byte>();

            var word = GetWord(address);

            byte c = (byte)(word >> 10 & 0x1f);
            chars.Add(c);
            c = (byte)(word >> 5 & 0x1f);
            chars.Add(c);
            c = (byte)(word >> 0 & 0x1f);
            chars.Add(c);

            return chars;
        }

        protected ushort GetWord(uint address)
        {
            return (ushort)(Machine.Memory[address] << 8 | Machine.Memory[address + 1]);
        }

    }
}