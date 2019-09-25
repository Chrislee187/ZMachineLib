using System.Collections.Generic;
using ZMachineLib.Content;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class OutputStream : ZMachineOperationBase
    {
        public OutputStream(IZMemory memory)
            : base((ushort)OpCodes.OutputStream, memory)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            // TODO
            Log.WriteLine("VarOp.OutputSteam To Be Implemented");
        }
    }
}