using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPExtended
{
    public sealed class LogShift : ZMachineOperationBase
    {
        public LogShift(IZMemory memory)
            : base((ushort)KindExtOpCodes.LogShift, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            // kill the sign bit, so make it a ushort
            var val = args[0];
            if ((short)args[1] > 0)
                val <<= args[1];
            else if ((short)args[1] < 0)
                val >>= -args[1];

            var dest = Contents.GetCurrentByteAndInc();
            Contents.VariableManager.Store(dest, val);
        }
    }
}