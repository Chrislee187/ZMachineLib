using System.Collections.Generic;
using ZMachineLib.Content;
using ZMachineLib.Operations;

namespace ZMachineLib
{
    public abstract class ZMachineOperationBase : IOperation
    {
        public ushort Code { get; }

        protected IZMemory Memory { get; }

        protected ZMachineOperationBase(ushort code, IZMemory memory)
        {
            Code = code;
            Memory = memory;
        }

        public abstract void Execute(List<ushort> args);

        protected void Call(List<ushort> args, bool storeResult)
        {
            if (args[0] == 0)
            {
                if (storeResult)
                {
                    var dest = Memory.GetCurrentByteAndInc();
                    Memory.VariableManager.Store(dest, 0);
                }

                return;
            }

            var pc = ZMemory.UnpackedAddress(args[0]);
            Log.Write($"New PC: {pc:X5}");

            var zsf = new ZStackFrame { PC = pc, StoreResult = storeResult };
            Memory.Stack.Push(zsf);

            var routineArgs = Memory.GetCurrentByteAndInc();

            InitialiseLocalVariables(routineArgs, zsf); // V4 Specific

            CopyArgsToLocalVariables(args, zsf);
        }

        private static void CopyArgsToLocalVariables(List<ushort> args, ZStackFrame zsf)
        {
            for (var i = 0; i < args.Count - 1; i++)
                zsf.Variables[i] = args[i + 1];

            zsf.ArgumentCount = args.Count - 1;
        }

        private void InitialiseLocalVariables(byte routineArgs, ZStackFrame zsf)
        {
            if (Memory.Header.Version <= 4)
            {
                for (var i = 0; i < routineArgs; i++)
                {
                    uint address = Memory.Stack.GetPC();
                    zsf.Variables[i] = Memory.Manager.GetUShort((int) address);
                    Memory.Stack.IncrementPC(2);
                }
            }
        }
    }
}