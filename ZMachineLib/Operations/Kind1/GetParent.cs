﻿using System.Collections.Generic;

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

            var addr = GetObjectAddress(args[0]);
            var parent = GetObjectNumber((ushort)(addr + Offsets.Parent));

            Log.Write($"[{GetObjectName(parent)}] ");

            var dest = Memory[Stack.Peek().PC++];

            if (Version <= 3)
                StoreByteInVariable(dest, (byte)parent);
            else
                StoreWordInVariable(dest, parent);
        }
    }
}