﻿using System.Collections.Generic;

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
            Log.Write($"[{ObjectManager.GetObjectName(args[0])}] ");

            var addr = ObjectManager.GetObjectAddress(args[0]);
            var sibling = ObjectManager.GetObjectNumber((ushort)(addr + Machine.VersionedOffsets.Sibling));

            Log.Write($"[{ObjectManager.GetObjectName(sibling)}] ");

            var dest = Machine.Memory[Machine.Stack.Peek().PC++];

            if (Machine.Header.Version <= 3)
            {
                byte value = (byte)sibling;
                VariableManager.StoreByte(dest, value);
            }
            else
                VariableManager.StoreWord(dest, sibling);

            Jump(sibling != 0);
        }
    }
}