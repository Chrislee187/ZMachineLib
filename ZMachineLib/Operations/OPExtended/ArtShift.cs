using System.Collections.Generic;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class ArtShift : ZMachineOperation
    {
        public ArtShift(ZMachine2 machine)
            : base((ushort)KindExtOpCodes.ArtShift, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            // keep the sign bit, so make it a short
            var val = (short)args[0];
            if ((short)args[1] > 0)
                val <<= args[1];
            else if ((short)args[1] < 0)
                val >>= -args[1];

            var dest = Memory[Stack.Peek().PC++];
            ushort value = (ushort)val;
            VarHandler.StoreWord(dest, value, true);
        }
    }
}