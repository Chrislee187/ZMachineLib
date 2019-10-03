using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class ArtShift : ZMachineOperationBase
    {
        public ArtShift(IZMemory memory)
            : base((ushort)KindExtOpCodes.ArtShift, memory)
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

            var dest = Memory.GetCurrentByteAndInc();
            ushort value = (ushort)val;
            Memory.VariableManager.Store(dest, value);
        }
    }
}