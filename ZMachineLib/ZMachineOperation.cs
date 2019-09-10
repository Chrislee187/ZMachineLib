using System.Collections.Generic;
using ZMachineLib.Operations;
using ZMachineLib.Operations.Kind0;

namespace ZMachineLib
{
    public abstract class ZMachineOperation : IOperation
    {
        public ushort Code { get; }
        
        protected readonly ZMachine2 Machine;

        protected byte[] Memory => Machine.Memory;
        protected Stack<ZStackFrame> Stack => Machine.Stack;

        protected void SetStack(Stack<ZStackFrame> newStack)
        {
            Machine.Stack = newStack;
        }
        protected ushort Version => Machine.Header.Version;
        private ushort Globals => Machine.Header.Globals;
        protected ushort ReadParseAddr
        {
            get => Machine.ReadParseAddr;
            set => Machine.ReadParseAddr = value;
        }

        protected ushort ReadTextAddr
        {
            get => Machine.ReadTextAddr;
            set => Machine.ReadTextAddr = value;
        }

        protected ushort ObjectTable => Machine.Header.ObjectTable;
        protected VersionedOffsets Offsets => Machine.VersionedOffsets;
        protected VersionedOffsets VersionedOffsets => Machine.VersionedOffsets;
        protected ZsciiString ZsciiString => Machine.ZsciiString;
        protected ushort DynamicMemorySize => Machine.Header.DynamicMemorySize;

        protected ZMachineOperation(ushort code,
            ZMachine2 machine)
        {
            Code = code;
            Machine = machine;
        }
        public abstract void Execute(List<ushort> args);

        protected ushort GetPropertyAddress(ushort obj, byte prop)
        {
            var propHeaderAddr = GetPropertyHeaderAddress(obj);

            // skip past text
            var size = Memory[propHeaderAddr];
            propHeaderAddr += (ushort)(size * 2 + 1);

            while (Memory[propHeaderAddr] != 0x00)
            {
                var propInfo = Memory[propHeaderAddr];
                var propNum = (byte)(propInfo & (Version <= 3 ? 0x1f : 0x3f));

                if (propNum == prop)
                    return propHeaderAddr;

                byte len;

                if (Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte)(Memory[++propHeaderAddr] & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte)((propInfo >> (Version <= 3 ? 5 : 6)) + 1);

                propHeaderAddr += (ushort)(len + 1);
            }

            return 0;
        }

        protected void StoreUint(uint address, uint val)
        {
            Memory[address + 0] = (byte)(val >> 24);
            Memory[address + 1] = (byte)(val >> 16);
            Memory[address + 2] = (byte)(val >> 8);
            Memory[address + 3] = (byte)(val >> 0);
        }

        protected uint GetUint(uint address)
        {
            return (uint)(Memory[address] << 24 | Memory[address + 1] << 16 | Memory[address + 2] << 8 |
                          Memory[address + 3]);
        }

        protected ushort PrintObjectInfo(ushort obj, bool properties)
        {
            if (obj == 0)
                return 0;

            var startAddr = GetObjectAddress(obj);

            var attributes = (ulong)GetUint(startAddr) << 16 | GetWord((uint)(startAddr + 4));
            var parent = GetObjectNumber((ushort)(startAddr + Offsets.Parent));
            var sibling = GetObjectNumber((ushort)(startAddr + Offsets.Sibling));
            var child = GetObjectNumber((ushort)(startAddr + Offsets.Child));
            var propAddr = GetWord((uint)(startAddr + Offsets.Property));

            Log.Write($"{obj} ({obj:X2}) at {propAddr:X5}: ");

            var size = Memory[propAddr++];
            var s = string.Empty;
            if (size > 0)
            {
                s = ZsciiString.GetZsciiString(propAddr);
            }

            propAddr += (ushort)(size * 2);

            Log.WriteLine(
                $"[{s}] A:{attributes:X12} P:{parent}({parent:X2}) ZsciiString:{sibling}({sibling:X2}) C:{child}({child:X2})");

            if (properties)
            {
                var ss = string.Empty;
                for (var i = 47; i >= 0; i--)
                {
                    if (((attributes >> i) & 0x01) == 0x01)
                    {
                        ss += 47 - i + ", ";
                    }
                }

                Log.WriteLine("Attributes: " + ss);

                while (Memory[propAddr] != 0x00)
                {
                    var propInfo = Memory[propAddr];
                    byte len;
                    if (Version > 3 && (propInfo & 0x80) == 0x80)
                        len = (byte)(Memory[propAddr + 1] & 0x3f);
                    else
                        len = (byte)((propInfo >> (Version <= 3 ? 5 : 6)) + 1);
                    var propNum = (byte)(propInfo & (Version <= 3 ? 0x1f : 0x3f));

                    Log.Write($"  P:{propNum:X2} at {propAddr:X4}: ");
                    for (var i = 0; i < len; i++)
                        Log.Write($"{Memory[propAddr++]:X2} ");
                    Log.WriteLine("");
                    propAddr++;
                }
            }

            return propAddr;
        }

        protected uint GetPackedAddress(ushort address)
        {
            if (Version <= 3)
                return (uint)(address * 2);
            if (Version <= 5)
                return (uint)(address * 4);

            return 0;
        }

        protected void SetObjectNumber(ushort objectAddr, ushort obj)
        {
            if (Version <= 3)
                Memory[objectAddr] = (byte)obj;
            else
                StoreWord(objectAddr, obj);
        }
        public void Call(List<ushort> args, bool storeResult)
        {
            if (args[0] == 0)
            {
                if (storeResult)
                {
                    var dest = Memory[Stack.Peek().PC++];
                    StoreWordInVariable(dest, 0);
                }

                return;
            }

            var pc = GetPackedAddress(args[0]);
            Log.Write($"New PC: {pc:X5}");

            var zsf = new ZStackFrame { PC = pc, StoreResult = storeResult };
            Stack.Push(zsf);

            var count = Memory[Stack.Peek().PC++];

            if (Version <= 4)
            {
                for (var i = 0; i < count; i++)
                {
                    zsf.Variables[i] = GetWord(Stack.Peek().PC);
                    Stack.Peek().PC += 2;
                }
            }

            for (var i = 0; i < args.Count - 1; i++)
                zsf.Variables[i] = args[i + 1];

            zsf.ArgumentCount = args.Count - 1;
        }

        protected ushort GetVariable(byte variable, bool pop = true)
        {
            ushort val;

            if (variable == 0)
            {
                if (pop)
                    val = Stack.Peek().RoutineStack.Pop();
                else
                    val = Stack.Peek().RoutineStack.Peek();
                Log.Write($"SP ({val:X4}), ");
            }
            else if (variable < 0x10)
            {
                val = Stack.Peek().Variables[variable - 1];
                Log.Write($"L{variable - 1:X2} ({val:X4}), ");
            }
            else
            {
                val = GetWord((ushort)(Globals + 2 * (variable - 0x10)));
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
                    Stack.Peek().RoutineStack.Pop();
                Stack.Peek().RoutineStack.Push(value);
            }
            else if (dest < 0x10)
            {
                Log.Write($"-> L{dest - 1:X2} ({value:X4}), ");
                Stack.Peek().Variables[dest - 1] = value;
            }
            else
            {
                Log.Write($"-> G{dest - 0x10:X2} ({value:X4}), ");
                StoreWord((ushort)(Globals + 2 * (dest - 0x10)), value);
            }
        }

        protected ushort GetObjectAddress(ushort obj)
        {
            var objectAddr = (ushort)(ObjectTable + Offsets.PropertyDefaultTableSize + (obj - 1) * Offsets.ObjectSize);
            return objectAddr;
        }

        protected string GetObjectName(ushort obj)
        {
            var s = string.Empty;

            if (obj != 0)
            {
                var addr = GetPropertyHeaderAddress(obj);
                if (Memory[addr] != 0)
                {
                    s = ZsciiString.GetZsciiString((uint)(addr + 1));
                }
            }

            return s;
        }

        protected ushort GetPropertyHeaderAddress(ushort obj)
        {
            var objectAddr = GetObjectAddress(obj);
            var propAddr = (ushort)(objectAddr + Offsets.Property);
            var prop = GetWord(propAddr);
            return prop;
        }

        protected ushort GetObjectNumber(ushort objectAddr)
        {
            if (Version <= 3)
                return Memory[objectAddr];
            return GetWord(objectAddr);
        }

        protected void StoreByteInVariable(byte dest, byte value)
        {
            if (dest == 0)
            {
                Log.Write($"-> SP ({value:X4}), ");
                Stack.Peek().RoutineStack.Push(value);
            }
            else if (dest < 0x10)
            {
                Log.Write($"-> L{dest - 1:X2} ({value:X4}), ");
                Stack.Peek().Variables[dest - 1] = value;
            }
            else
            {
                // this still gets written as a word...write the byte to addr+1
                Log.Write($"-> G{dest - 0x10:X2} ({value:X4}), ");
                Memory[Globals + 2 * (dest - 0x10)] = 0;
                Memory[Globals + 2 * (dest - 0x10) + 1] = value;
            }
        }

        protected void StoreWord(ushort address, ushort value)
        {
            Memory[address + 0] = (byte)(value >> 8);
            Memory[address + 1] = (byte)value;
        }

        protected void Jump(bool flag)
        {
            bool branch;

            var offset = Memory[Stack.Peek().PC++];
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
                var offset2 = Memory[Stack.Peek().PC++];
                var final = (ushort)((offset & 0x3f) << 8 | offset2);

                // this is a 14-bit number, so set the sign bit properly because we can jump backwards
                if ((final & 0x2000) == 0x2000)
                    final |= 0xc000;

                newOffset = (short)(final - 2);
            }

            if (executeBranch)
                Stack.Peek().PC += (uint)newOffset;

            Log.Write($"-> { Stack.Peek().PC:X5}");
        }


        protected List<byte> GetZsciiChars(uint address)
        {
            var chars = new List<byte>();
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
            var chars = new List<byte>();

            var word = GetWord(address);

            var c = (byte)(word >> 10 & 0x1f);
            chars.Add(c);
            c = (byte)(word >> 5 & 0x1f);
            chars.Add(c);
            c = (byte)(word >> 0 & 0x1f);
            chars.Add(c);

            return chars;
        }

        protected ushort GetWord(uint address)
        {
            return (ushort)(Memory[address] << 8 | Memory[address + 1]);
        }
    }
}