using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mod : ZMachineOperationBase
    {
        public Mod(IZMemory contents)
            : base((ushort)OpCodes.Mod, contents)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var val = (short)((short)args[0] % (short)args[1]);
            var dest = Contents.GetCurrentByteAndInc();
            ushort value = (ushort)val;
            Contents.VariableManager.Store(dest, value);
        }
    }
}