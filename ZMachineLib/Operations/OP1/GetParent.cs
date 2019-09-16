using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class GetParent : ZMachineOperation
    {
        public GetParent(ZMachine2 machine)
            : base((ushort)OpCodes.GetParent, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            var addr = GetObjectAddress(args[0]);
            var parent = GetObjectNumber((ushort)(addr + Offsets.Parent));

            Log.Write($"[{GetObjectName(parent)}] ");

            var dest = Memory[Stack.Peek().PC++];

            if (Version <= 3)
            {
                byte value = (byte)parent;
                VarHandler.StoreByte(dest, value);
            }
            else
                VarHandler.StoreWord(dest, parent, true);
        }
    }
}