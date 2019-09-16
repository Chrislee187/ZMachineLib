using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class GetChild : ZMachineOperation
    {
        public GetChild(ZMachine2 machine)
            : base((ushort)OpCodes.GetChild, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            var addr = GetObjectAddress(args[0]);
            var child = GetObjectNumber((ushort)(addr + Offsets.Child));

            Log.Write($"[{GetObjectName(child)}] ");

            var dest = Memory[Stack.Peek().PC++];

            if (Version <= 3)
                StoreByteInVariable(dest, (byte)child);
            else
                StoreWordInVariable(dest, child);

            Jump(child != 0);
        }
    }
}