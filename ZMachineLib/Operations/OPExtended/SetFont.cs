using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class SetFont : ZMachineOperationBase
    {
        public SetFont(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.SetFont, machine.Memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            // TODO

            var dest = GetCurrentByteAndInc();
            Contents.VariableManager.StoreWord(dest, 0);
        }
    }
}