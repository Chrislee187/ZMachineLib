using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class SetFont : ZMachineOperationBase
    {
        public SetFont(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.SetFont, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            // TODO

            var dest = PeekNextByte();
            VariableManager.StoreWord(dest, 0);
        }
    }
}