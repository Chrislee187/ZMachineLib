using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class StoreB : ZMachineOperation
    {
        public StoreB(ZMachine2 machine)
            : base((ushort)OpCodes.StoreB, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var addr = (ushort)(operands[0] + operands[1]);
            Machine.Memory[addr] = (byte)operands[2];
        }
    }
}