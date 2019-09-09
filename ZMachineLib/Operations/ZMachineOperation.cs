using System;
using System.Collections.Generic;
using ZMachineLib.Operations.Kind0;

namespace ZMachineLib.Operations
{
    public abstract class ZMachineOperation : IOperation
    {
        public ushort Code { get; }
        
        protected const ushort ZFalse = 0;
        protected const ushort ZTrue = 1;

        protected readonly ZMachine2 Machine;

        protected ZMachineOperation(ushort code,
            ZMachine2 machine)
        {
            Code = code;
            Machine = machine;
        }
        public abstract void Execute(List<ushort> args);

        protected uint GetPackedAddress(ushort address)
        {
            if (Machine.Version <= 3)
                return (uint)(address * 2);
            if (Machine.Version <= 5)
                return (uint)(address * 4);

            return 0;
        }

        protected void SetObjectNumber(ushort objectAddr, ushort obj)
        {
            if (Machine.Version <= 3)
                Machine.Memory[objectAddr] = (byte)obj;
            else
                StoreWord(objectAddr, obj);
        }
        public void Call(List<ushort> args, bool storeResult)
        {
            if (args[0] == 0)
            {
                if (storeResult)
                {
                    byte dest = Machine.Memory[Machine.Stack.Peek().PC++];
                    StoreWordInVariable(dest, 0);
                }

                return;
            }

            uint pc = GetPackedAddress(args[0]);
            Log.Write($"New PC: {pc:X5}");

            ZStackFrame zsf = new ZStackFrame { PC = pc, StoreResult = storeResult };
            Machine.Stack.Push(zsf);

            byte count = Machine.Memory[Machine.Stack.Peek().PC++];

            if (Machine.Version <= 4)
            {
                for (int i = 0; i < count; i++)
                {
                    zsf.Variables[i] = GetWord(Machine.Stack.Peek().PC);
                    Machine.Stack.Peek().PC += 2;
                }
            }

            for (int i = 0; i < args.Count - 1; i++)
                zsf.Variables[i] = args[i + 1];

            zsf.ArgumentCount = args.Count - 1;
        }

        protected ushort GetVariable(byte variable, bool pop = true)
        {
            ushort val;

            if (variable == 0)
            {
                if (pop)
                    val = Machine.Stack.Peek().RoutineStack.Pop();
                else
                    val = Machine.Stack.Peek().RoutineStack.Peek();
                Log.Write($"SP ({val:X4}), ");
            }
            else if (variable < 0x10)
            {
                val = Machine.Stack.Peek().Variables[variable - 1];
                Log.Write($"L{variable - 1:X2} ({val:X4}), ");
            }
            else
            {
                val = GetWord((ushort)(Machine.Globals + 2 * (variable - 0x10)));
                Log.Write($"G{variable - 0x10:X2} ({val:X4}), ");
            }

            return val;
        }


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

        protected ushort GetObjectAddress(ushort obj)
        {
            ushort objectAddr = (ushort)(Machine.ObjectTable + Machine.Offsets.PropertyDefaultTableSize + (obj - 1) * Machine.Offsets.ObjectSize);
            return objectAddr;
        }

        protected string GetObjectName(ushort obj)
        {
            string s = string.Empty;

            if (obj != 0)
            {
                ushort addr = GetPropertyHeaderAddress(obj);
                if (Machine.Memory[addr] != 0)
                {
                    s = Machine.ZsciiString.GetZsciiString((uint)(addr + 1));
                }
            }

            return s;
        }

        protected ushort GetPropertyHeaderAddress(ushort obj)
        {
            ushort objectAddr = GetObjectAddress(obj);
            ushort propAddr = (ushort)(objectAddr + Machine.Offsets.Property);
            ushort prop = GetWord(propAddr);
            return prop;
        }

        protected ushort GetObjectNumber(ushort objectAddr)
        {
            if (Machine.Version <= 3)
                return Machine.Memory[objectAddr];
            return GetWord(objectAddr);
        }

        protected void StoreByteInVariable(byte dest, byte value)
        {
            if (dest == 0)
            {
                Log.Write($"-> SP ({value:X4}), ");
                Machine.Stack.Peek().RoutineStack.Push(value);
            }
            else if (dest < 0x10)
            {
                Log.Write($"-> L{dest - 1:X2} ({value:X4}), ");
                Machine.Stack.Peek().Variables[dest - 1] = value;
            }
            else
            {
                // this still gets written as a word...write the byte to addr+1
                Log.Write($"-> G{dest - 0x10:X2} ({value:X4}), ");
                Machine.Memory[Machine.Globals + 2 * (dest - 0x10)] = 0;
                Machine.Memory[Machine.Globals + 2 * (dest - 0x10) + 1] = value;
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