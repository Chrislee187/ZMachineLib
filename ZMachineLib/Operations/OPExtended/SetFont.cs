using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class SetFont : ZMachineOperation
    {
        public SetFont(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.SetFont, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            // TODO

            var dest = GetNextByte();
            VariableManager.StoreWord(dest, 0);
        }
    }
}