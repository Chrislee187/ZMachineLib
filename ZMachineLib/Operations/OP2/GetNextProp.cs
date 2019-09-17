﻿using System.Collections.Generic;

namespace ZMachineLib.Operations.OP2
{
    public sealed class GetNextProp : ZMachineOperation
    {
        public GetNextProp(ZMachine2 machine)
            : base((ushort)OpCodes.GetNextProp, machine)
        {
        }

        public override void Execute(List<ushort> operands)
        {
            Log.Write($"[{ObjectManager.GetObjectName(operands[0])}] ");

            var next = false;

            var dest = PeekNextByte();
            if (operands[1] == 0)
                next = true;

            var propHeaderAddr = ObjectManager.GetPropertyHeaderAddress(operands[0]);
            var size = Machine.Memory[propHeaderAddr];
            propHeaderAddr += (ushort)(size * 2 + 1);

            while (Machine.Memory[propHeaderAddr] != 0x00)
            {
                var propInfo = Machine.Memory[propHeaderAddr];
                byte len;
                if (Machine.Header.Version > 3 && (propInfo & 0x80) == 0x80)
                {
                    len = (byte)(Machine.Memory[++propHeaderAddr] & 0x3f);
                    if (len == 0)
                        len = 64;
                }
                else
                    len = (byte)((propInfo >> ((ushort) Machine.Header.Version <= 3 ? 5 : 6)) + 1);

                var propNum = (byte)(propInfo & ((ushort) Machine.Header.Version <= 3 ? 0x1f : 0x3f));

                if (next)
                {
                    VariableManager.StoreByte(dest, propNum);
                    return;
                }

                if (propNum == operands[1])
                    next = true;

                propHeaderAddr += (ushort)(len + 1);
            }

            VariableManager.StoreByte(dest, 0);
        }
    }
}