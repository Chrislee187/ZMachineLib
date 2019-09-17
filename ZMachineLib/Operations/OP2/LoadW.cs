using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OP2
{
    public sealed class LoadW : ZMachineOperation
    {
        public LoadW(ZMachine2 machine)
            : base((ushort)OpCodes.LoadW, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + 2 * operands[1]);
            var dest = PeekNextByte();
            VariableManager.StoreWord(dest, Machine.Memory.GetUshort(addr));
        }
    }
}