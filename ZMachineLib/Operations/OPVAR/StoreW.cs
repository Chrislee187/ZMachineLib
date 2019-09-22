using System.Collections.Generic;
using ZMachineLib.Extensions;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class StoreW : ZMachineOperationBase
    {
        public StoreW(ZMachine2 machine)
            : base((ushort)OpCodes.StoreW, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + 2 * operands[1]);
            ushort value = operands[2];
            Machine.Memory.SetWord(addr, value);
        }
    }
}