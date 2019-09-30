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
            var dest = Contents.GetCurrentByteAndInc();
            var result = args[0] % args[1];

            OpLogging.Op2WithStore(GetType().Name.ToUpper(), args[0], args[1], result, dest);

            Contents.VariableManager.Store(dest, (ushort) result);
        }
    }
}