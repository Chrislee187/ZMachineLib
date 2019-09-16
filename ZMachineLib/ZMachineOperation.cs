using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZMachineLib.Extensions;
using ZMachineLib.Operations;

namespace ZMachineLib
{
    public abstract class ZMachineOperation : IOperation
    {
        
        public ushort Code { get; }
        
        protected readonly ZMachine2 Machine;
        protected readonly VarHandler VarHandler;

        private ushort GlobalsTable => Machine.Header.Globals;

        protected byte[] Memory => Machine.Memory;
        protected Stack<ZStackFrame> Stack => Machine.Stack;

        protected void SetStack(Stack<ZStackFrame> newStack)
        {
            Machine.Stack = newStack;
        }
        protected ushort Version => Machine.Header.Version;
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
            VarHandler = new VarHandler(Machine);
        }
        public abstract void Execute(List<ushort> args);

        protected void Call(List<ushort> args, bool storeResult)
        {
            if (args[0] == 0)
            {
                if (storeResult)
                {
                    var dest = Memory[Stack.Peek().PC++];
                    VarHandler.StoreWord(dest, 0, true);
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
                    uint address = Stack.Peek().PC;
                    zsf.Variables[i] = Machine.Memory.GetUshort(address);
                    Stack.Peek().PC += 2;
                }
            }

            for (var i = 0; i < args.Count - 1; i++)
                zsf.Variables[i] = args[i + 1];

            zsf.ArgumentCount = args.Count - 1;
        }

        protected ushort GetPropertyHeaderAddress(ushort obj)
        {
            var objectAddr = GetObjectAddress(obj);
            var propAddr = (ushort)(objectAddr + Offsets.Property);
            var prop = Machine.Memory.GetUshort(propAddr);
            return prop;
        }

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

        protected ushort PrintObjectInfo(ushort obj, bool properties)
        {
            if (obj == 0)
                return 0;

            var startAddr = GetObjectAddress(obj);

            var attributes = (ulong)Memory.GetUInt(startAddr) << 16 | Machine.Memory.GetUshort((uint)(startAddr + 4));
            var parent = GetObjectNumber((ushort)(startAddr + Offsets.Parent));
            var sibling = GetObjectNumber((ushort)(startAddr + Offsets.Sibling));
            var child = GetObjectNumber((ushort)(startAddr + Offsets.Child));
            var propAddr = Machine.Memory.GetUshort((uint)(startAddr + Offsets.Property));

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
                Memory.StoreAt(objectAddr, obj);
        }
        protected ushort GetObjectNumber(ushort objectAddr)
        {
            if (Version <= 3)
                return Memory[objectAddr];
            return Machine.Memory.GetUshort(objectAddr);
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

        private Action<bool> _customJump = null;

        public Action<bool> Jump
        {
            protected get => _customJump ?? JumpImpl;
            set => _customJump = value;
        }

        private void JumpImpl(bool flag)
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
    }
}