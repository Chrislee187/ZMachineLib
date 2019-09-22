using System;
using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Extensions;
using ZMachineLib.Managers;
using ZMachineLib.Operations;

namespace ZMachineLib
{
    public abstract class ZMachineOperationBase : IOperation
    {
        public ushort Code { get; }
        
        protected readonly ZMachine2 Machine;
        protected readonly IVariableManager VariableManager;
        protected readonly IObjectManager ObjectManager;
        protected readonly IMemoryManager MemoryManager;

        protected ZMachineOperationBase(ushort code,
            ZMachine2 machine,
            IObjectManager objectManager = null,
            IVariableManager variableManager = null,
            IMemoryManager memoryManager = null)
        {
            Code = code;
            Machine = machine;
            if (Machine != null)
            {
                MemoryManager = memoryManager ?? new MemoryManager(Machine.Contents?.Memory);
                ObjectManager = objectManager ?? new ObjectManager(Machine, Machine.Abbreviations, MemoryManager);
                VariableManager = variableManager ?? new VariableManager(Machine, MemoryManager);
            }
        }

        public abstract void Execute(List<ushort> operands);

        protected void Call(List<ushort> args, bool storeResult)
        {
            if (args[0] == 0)
            {
                if (storeResult)
                {
                    var dest = PeekNextByte();
                    VariableManager.StoreWord(dest, 0);
                }

                return;
            }

            var pc = ObjectManager.GetPackedAddress(args[0]);
            Log.Write($"New PC: {pc:X5}");

            var zsf = new ZStackFrame { PC = pc, StoreResult = storeResult };
            Machine.Stack.Push(zsf);

            var count = PeekNextByte();

            if (Machine.Header.Version <= 4)
            {
                for (var i = 0; i < count; i++)
                {
                    uint address = Machine.Stack.Peek().PC;
                    zsf.Variables[i] = Machine.Memory.GetUShort((int) address);
                    Machine.Stack.Peek().PC += 2;
                }
            }

            for (var i = 0; i < args.Count - 1; i++)
                zsf.Variables[i] = args[i + 1];

            zsf.ArgumentCount = args.Count - 1;
        }


        // NOTE: Slightly funky setup for Jump and PeekNextByte
        // so we can replace them when testing
        private Action<bool> _customJump;
        public Action<bool> Jump
        {
            protected get => _customJump ?? JumpImpl;
            set => _customJump = value;
        }
        private void JumpImpl(bool flag)
        {
            bool branch;

            var offset = PeekNextByte();
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
                var offset2 = PeekNextByte();
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

        private Func<byte> _customPeekNextByte;
        public Func<byte> PeekNextByte
        {
            protected get => _customPeekNextByte ?? PeekNextByteImpl;
            set => _customPeekNextByte = value;
        }
        private byte PeekNextByteImpl()
            => MemoryManager.Get( Machine.Stack.Peek().PC++ );
    }
}