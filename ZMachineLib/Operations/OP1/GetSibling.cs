using System.Collections.Generic;

namespace ZMachineLib.Operations.OP1
{
    public sealed class GetSibling : ZMachineOperation
    {
        public GetSibling(ZMachine2 machine)
            : base((ushort)OpCodes.GetSibling, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            var addr = GetObjectAddress(args[0]);
            var sibling = GetObjectNumber((ushort)(addr + VersionedOffsets.Sibling));

            Log.Write($"[{GetObjectName(sibling)}] ");

            var dest = Memory[Stack.Peek().PC++];

            if (Version <= 3)
            {
                byte value = (byte)sibling;
                VarHandler.StoreByte(dest, value);
            }
            else
                VarHandler.StoreWord(dest, sibling, true);

            Jump(sibling != 0);
        }
    }
}