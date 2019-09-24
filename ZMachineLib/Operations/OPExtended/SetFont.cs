using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class SetFont : ZMachineOperationBase
    {
        public SetFont(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.SetFont, machine, machine.Contents)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            // TODO

            var dest = GetNextByte();
            Contents.VariableManager.StoreWord(dest, 0);
        }
    }
}