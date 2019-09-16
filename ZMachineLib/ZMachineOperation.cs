using System;
using System.Collections.Generic;
using ZMachineLib.Extensions;
using ZMachineLib.Operations;

namespace ZMachineLib
{
    public abstract class ZMachineOperation : IOperation
    {
        
        public ushort Code { get; }
        
        protected readonly ZMachine2 Machine;
        protected readonly VariableManager VariableManager;
        protected readonly ObjectManager ObjectManager;

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
            ObjectManager = new ObjectManager(Machine);
            VariableManager = new VariableManager(Machine);
        }
        public abstract void Execute(List<ushort> args);

        protected void Call(List<ushort> args, bool storeResult)
        {
            if (args[0] == 0)
            {
                if (storeResult)
                {
                    var dest = Memory[Stack.Peek().PC++];
                    VariableManager.StoreWord(dest, 0);
                }

                return;
            }

            var pc = ObjectManager.GetPackedAddress(args[0]);
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

        private Action<bool> _customJump;

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