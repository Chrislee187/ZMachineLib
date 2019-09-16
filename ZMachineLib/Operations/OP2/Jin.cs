using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class Jin : ZMachineOperation
    {
        public Jin(ZMachine2 machine)
            : base((ushort)OpCodes.Jin, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"C[{GetObjectName(args[0])}] P[{GetObjectName(args[1])}] ");

            var addr = GetObjectAddress(args[0]);
            var parent = GetObjectNumber((ushort)(addr + Offsets.Parent));
            Jump(parent == args[1]);
        }
    }
}