using System;
using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Operations;

namespace ZMachineLib
{
    public abstract class ZMachineOperationBase : IOperation
    {
        public ushort Code { get; }

        protected IZMemory Contents { get; }

        protected ZMachineOperationBase(ushort code, IZMemory contents)
        {
            Code = code;
            Contents = contents;
        }

        public abstract void Execute(List<ushort> args);

        protected void Call(List<ushort> args, bool storeResult)
        {
            if (args[0] == 0)
            {
                if (storeResult)
                {
                    var dest = Contents.GetCurrentByteAndInc();
                    Contents.VariableManager.Store(dest, 0);
                }

                return;
            }

            var pc = Contents.GetPackedAddress(args[0]);
            Log.Write($"New PC: {pc:X5}");

            var zsf = new ZStackFrame { PC = pc, StoreResult = storeResult };
            Contents.Stack.Push(zsf);

            var count = Contents.GetCurrentByteAndInc();

            if (Contents.Header.Version <= 4)
            {
                for (var i = 0; i < count; i++)
                {
                    uint address = Contents.Stack.GetPC();
                    zsf.Variables[i] = Contents.Manager.GetUShort((int) address);
                    Contents.Stack.IncrementPC(2);
                }
            }

            for (var i = 0; i < args.Count - 1; i++)
                zsf.Variables[i] = args[i + 1];

            zsf.ArgumentCount = args.Count - 1;
        }

        // NOTE: Slightly funky setup for Jump and GetCurrentByteAndInc
        // so we can replace them when testing
        private Action<bool> _customJump;
        public Action<bool> Jump
        {
            protected get => _customJump ?? JumpImpl;
            set => _customJump = value;
        }
        private void JumpImpl(bool flag)
        {

            var offset = Contents.GetCurrentByteAndInc();
            short newOffset;

            bool branch = (offset & 0x80) == 0x80;
            Log.Write($" [{branch.ToString().ToUpper()}] ");

            var executeBranch = branch && flag || !(branch || flag);

            if ((offset & 0x40) == 0x40)
            {
                offset = (byte)(offset & 0x3f);

                if (offset == 0 && executeBranch)
                {
                    Log.Write(" RFALSE ");
                    OpCodeRBoolean(false);
                    return;
                }

                if (offset == 1 && executeBranch)
                {
                    Log.Write(" RTRUE ");
                    OpCodeRBoolean(true);
                    return;
                }

                newOffset = (short)(offset - 2);
            }
            else
            {
                var offset2 = Contents.GetCurrentByteAndInc();
                var final = (ushort)((offset & 0x3f) << 8 | offset2);

                // this is a 14-bit number, so set the sign bit properly because we can jump backwards
                if ((final & 0x2000) == 0x2000)
                    final |= 0xc000;

                newOffset = (short)(final - 2);
            }

            if (executeBranch)
                Contents.Stack.IncrementPC((uint)newOffset);

            Log.Write($"-> { Contents.Stack.GetPC():X5}");
        }

        private void OpCodeRBoolean(bool val)
        {
            if (Contents.Stack.Pop().StoreResult)
            {
                Contents.VariableManager.Store(Contents.GetCurrentByteAndInc(), (ushort) (val ? 1 : 0));
            }
        }
    }
}