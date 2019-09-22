using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP1
{
    public sealed class Ret : ZMachineOperationBase
    {
        public Ret(ZMachine2 machine)
            : base((ushort) OpCodes.Ret, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var sf = Machine.Stack.Pop();
            if (sf.StoreResult)
            {
                var dest = GetNextByte();
                ushort value = operands[0];
                Contents.VariableManager.StoreWord(dest, value);
            }
        }
    }
}