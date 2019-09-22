using System.Collections.Generic;
using ZMachineLib.Managers;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mod : ZMachineOperationBase
    {
        public Mod(ZMachine2 machine)
            : base((ushort)OpCodes.Mod, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            var val = (short)((short)operands[0] % (short)operands[1]);
            var dest = GetNextByte();
            ushort value = (ushort)val;
            Contents.VariableManager.StoreWord(dest, value);
        }
    }
}