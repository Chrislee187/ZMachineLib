using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class GetSibling : ZMachineOperation
    {
        public GetSibling(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.GetSibling, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            ushort addr = GetObjectAddress(args[0]);
            ushort sibling = GetObjectNumber((ushort)(addr + VersionOffsets.Sibling));

            Log.Write($"[{GetObjectName(sibling)}] ");

            byte dest = Memory[Stack.Peek().PC++];

            if (Version <= 3)
                StoreByteInVariable(dest, (byte)sibling);
            else
                StoreWordInVariable(dest, sibling);

            Jump(sibling != 0);
        }
    }
}