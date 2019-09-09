using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public sealed class GetParent : ZMachineOperation
    {
        public GetParent(ZMachine2 machine)
            : base((ushort)Kind1OpCodes.GetParent, machine)
        {
        }

        public override void Execute(List<ushort> args)
        {
            Log.Write($"[{GetObjectName(args[0])}] ");

            ushort addr = GetObjectAddress(args[0]);
            ushort parent = GetObjectNumber((ushort)(addr + Machine.Offsets.Parent));

            Log.Write($"[{GetObjectName(parent)}] ");

            byte dest = Machine.Memory[Machine.Stack.Peek().PC++];

            if (Machine.Version <= 3)
                StoreByteInVariable(dest, (byte)parent);
            else
                StoreWordInVariable(dest, parent);
        }
    }
}