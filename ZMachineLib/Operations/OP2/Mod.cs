using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Mod : ZMachineOperationBase
    {
        public Mod(IZMemory memory)
            : base((ushort)OpCodes.Mod, memory)
        {
        }

        public override void Execute(List<ushort> args)
        {
            var dest = Memory.GetCurrentByteAndInc();
            var result = args[0] % args[1];

            OpLogging.Op2WithStore(GetType().Name.ToUpper(), args[0], args[1], result, dest);

            Memory.VariableManager.Store(dest, (ushort) result);
        }
    }
}