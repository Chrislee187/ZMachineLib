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

            var pc = ZMemory.GetPackedAddress(args[0]);
            Log.Write($"New PC: {pc:X5}");

            var zsf = new ZStackFrame { PC = pc, StoreResult = storeResult };
            Contents.Stack.Push(zsf);

            var routineArgs = Contents.GetCurrentByteAndInc();

            InitialiseArgs(routineArgs, zsf); // V4 Specific

            CopyArgsToLocalVariables(args, zsf);
        }

        private static void CopyArgsToLocalVariables(List<ushort> args, ZStackFrame zsf)
        {
            for (var i = 0; i < args.Count - 1; i++)
                zsf.Variables[i] = args[i + 1];

            zsf.ArgumentCount = args.Count - 1;
        }

        private void InitialiseArgs(byte routineArgs, ZStackFrame zsf)
        {
            if (Contents.Header.Version <= 4)
            {
                for (var i = 0; i < routineArgs; i++)
                {
                    uint address = Contents.Stack.GetPC();
                    zsf.Variables[i] = Contents.Manager.GetUShort((int) address);
                    Contents.Stack.IncrementPC(2);
                }
            }
        }
    }
}