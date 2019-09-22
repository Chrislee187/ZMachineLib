using System.Collections.Generic;

namespace ZMachineLib.Operations.OPVAR
{
    public sealed class OutputStream : ZMachineOperationBase
    {
        public OutputStream(ZMachine2 machine)
            : base((ushort)OpCodes.OutputStream, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            // TODO
            Log.WriteLine("VarOp.OutputSteam To Be Implemented");
        }
    }
}