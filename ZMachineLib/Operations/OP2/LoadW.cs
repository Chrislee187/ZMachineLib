using System.Collections.Generic;
using ZMachineLib.Extensions;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    public sealed class LoadW : ZMachineOperationBase
    {
        public LoadW(ZMachine2 machine)
            : base((ushort)OpCodes.LoadW, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + 2 * operands[1]);
            var dest = GetNextByte();
            Contents.VariableManager.StoreWord(dest, Machine.Memory.GetUShort(addr));
        }
    }
}