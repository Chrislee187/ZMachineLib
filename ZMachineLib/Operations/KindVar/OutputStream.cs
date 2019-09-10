using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public sealed class OutputStream : ZMachineOperation
    {
        public OutputStream(ZMachine2 machine)
            : base((ushort)KindVarOpCodes.OutputStream, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            // TODO
            Log.WriteLine("VarOp.OutputSteam To Be Implemented");
        }
    }
}